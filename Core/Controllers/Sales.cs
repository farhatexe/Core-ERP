using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core.Controllers
{
    public class Sales
    {

        private Context _db;

        public Sales(Context db)
        {
            _db = db;
        }

        /// <summary>
        /// All the Orders brought by the list.
        /// </summary>
        /// <value>Orders (Purchases or Sales)</value>
        List<Models.Order> Orders { get; set; }

        /// <summary>
        /// List this instance.
        /// </summary>
        /// <returns>The list.</returns>
        public ObservableCollection<Models.Order> List(int take = 25, int skip = 0, bool includeArchived = false)
        {
            if (includeArchived)
            {
                _db.Orders.Include(x => x.details).Take(take).Skip(skip).Load();
            }
            else
            {
                _db.Orders.Where(x => x.isArchived == false)
                    .Include(x=>x.details)
                    .Take(take)
                    .Skip(skip)
                    .Load();
            }

            return _db.Orders.Local.ToObservableCollection();
        }


        public IEnumerable<object> ListItem()
        {

            IEnumerable<object> query = from Item in _db.Items.DefaultIfEmpty()
                                        join Movements in _db.ItemMovements on Item equals Movements.item into itemStock
                                        from Is in itemStock
                                        group Is by Is.item into g
                                        select new
                                        {
                                            Item = g.Key.localId,
                                            Balance = g.Key.ItemMovements.Sum(x => x.credit - x.debit),
                                            Cost = g.Key.cost
                                        };

            return query;
        }

        /// <summary>
        /// Adds the detail.
        /// </summary>
        /// <returns>The detail.</returns>
        /// <param name="order">Order.</param>
        /// <param name="Item">Item.</param>
        /// <param name="quantity">Quantity.</param>
        public Core.Models.OrderDetail AddDetail(Models.Order order, dynamic Item, int quantity = 1)
        {
            Models.OrderDetail OrderDetail = new OrderDetail();
            OrderDetail.order = order;
            OrderDetail.item = Item;
            if (Item.vatCloudId != null)
            {
                int id = (int)Item.vatCloudId;
                OrderDetail.vat = _db.Vats.Include(x=>x.details).Where(x => x.cloudId == id).FirstOrDefault();
            }

            OrderDetail.quantity = quantity;
            OrderDetail.price = OrderDetail.item.price;
                

            OrderDetail.discount = 0;
            OrderDetail.QuantityUpdated = false;
            return OrderDetail;
        }

        public Models.Order Add(Models.Session session, Core.Models.Location location = null, Core.Models.PaymentContract paymentContract = null)
        {

            Core.Models.Order Order = new Core.Models.Order();
            Order.status = Enums.Status.Pending;
            //TODO Select proper location...
            location = _db.Locations.FirstOrDefault();
            //TODO Select Proper company...
            Order.company = _db.Companies.FirstOrDefault();
            //TODO Select proper location...
            Order.location = location;
            //TODO Select proper Paymnetcontract
            Order.paymentContract = _db.PaymentContracts.FirstOrDefault();
            Order.session = session;
            Order.range = location.ranges.Where(x => x.expiryDate > DateTime.Now && x.endValue > x.currentValue).FirstOrDefault();
            return Order;
        }

        public Models.Order AddRemoveVat(Models.Order order, bool RemoveVat = true)
        {
            foreach (Core.Models.OrderDetail Detail in order.details)
            {
                if (RemoveVat)
                {
                    Detail.vat = null;
                }
                else
                {
                    Detail.vat = Detail.item.vat;
                }
                Detail.RaisePropertyChanged("vat");
                Detail.RaisePropertyChanged("priceVat");
                Detail.RaisePropertyChanged("subTotal");
                Detail.RaisePropertyChanged("subTotalVat");
            }
            order.RaisePropertyChanged("total");
            return order;
        }
        /// <summary>
        /// Creates a new instance of the Order.
        /// </summary>
        /// <returns>The new.</returns>
        public Models.Order New() => new Models.Order();

        /// <summary>
        /// Save the specified Order.
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="Order">Order.</param>
        public Boolean Save(Models.Order Order)
        {
            foreach (OrderDetail detail in Order.details)
            {
                int id = (int)detail.item.localId;
                Item Item = _db.Items.Where(x => x.localId == id).FirstOrDefault();
                detail.item = Item;
            }
            _db.Orders.Add(Order);

            _db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Approve the specified Order, RecalculatePrices and IgnoreErrors.
        /// If successful function will return Order (Saved) and allow you to use this to print physical Invoice.
        /// </summary>
        /// <param name="Order">Order</param>
        /// <param name="RecalculatePrices">If set to <c>true</c> recalculate prices.</param>
        /// <param name="IgnoreErrors">If set to <c>true</c> ignore errors.</param>
        public Models.Order Approve(Models.Order Order, bool IgnoreErrors = false, bool InsertSchedual = true, bool RecalculatePrices = false)
        {




            //Validate Stock Levels,
            foreach (var detail in Order.details.Where(x => x.item.isStockable))
            {
                // Check stock levels of each item for that location.
                // TODO: place this code into item controller and re-use code from there.
                decimal InStock = _db.ItemMovements
                                         .Where(x => x.location == Order.location && x.item == detail.item)
                                         .Sum(y => y.debit - y.credit);

                //If Stock is less than or equal to 0, send message of OutOfStock
                if (InStock <= 0)
                {
                    detail.message = Message.Warning.OutOfStock;
                    Order.message = Message.Warning.OutOfStock;
                }
            }

            //Check if Order Range exist and is out of range.
            if (Order.range != null)
            {

                if (Order.range.expiryDate != null && Order.range.expiryDate < Order.date)
                {
                    Order.message = Message.Warning.OutOfDocumentRange;
                }
                else if (Order.range.endValue <= Order.range.currentValue)
                {
                    Order.message = Message.Warning.OutOfDocumentRange;
                }
                else
                {
                    Order.invoiceNumber = Order.range.document.numberTemplate + (Order.range.currentValue);
                    Order.range.currentValue = Order.range.currentValue + 1;
                }
            }
          

            //If IgnoreErrors is False and Error message shows up, return without doing any work.
            if (IgnoreErrors == false && Order.message != null)
            {
                return null;
            }

            //Insert into Stock Movements
            foreach (var detail in Order.details.Where(x => x.item.isStockable))
            {
                //TODO: take this code to ItemMovement Unit Of Work.
                Models.ItemMovement Movement = new Models.ItemMovement()
                {
                    item = detail.item,
                    date = Order.date,
                    credit = 0,
                    debit = detail.quantity,
                    location = Order.location
                };

                _db.ItemMovements.Add(Movement);
            }

            //Check Prices
            if (RecalculatePrices)
            {
                ItemPromotion promo = new ItemPromotion(_db);
                promo.CalculatePromotionsOnSales(Order);

            }

            //Insert into Payment Schedual
            if (Order.paymentContract != null)
            {
                //Loop through PaymentContract Detail to break down payment requirement.
                foreach (var paymentDetail in Order.paymentContract.details.Where(x => x.forOrders == false))
                {
                    Models.PaymentSchedual schedual = new Models.PaymentSchedual()
                    {
                        order = Order,
                        date = Order.date.AddDays(paymentDetail.offset),
                        amountOwed = Order.details.Sum(x => x.subTotalVat) * paymentDetail.percentage,
                        comment = Order.invoiceNumber
                    };

                    _db.PaymentSchedual.Add(schedual);
                }
            }
            else if (InsertSchedual)
            {

                //Incase Payment Contract is not established.
                Models.PaymentSchedual schedual = new Models.PaymentSchedual()
                {
                    order = Order,
                    date = Order.date,
                    amountOwed = Order.details.Sum(x => x.subTotalVat),
                    comment = Order.invoiceNumber
                };

                _db.PaymentSchedual.Add(schedual);
            }






            _db.SaveChanges();

            return Order;
        }

        /// <summary>
        /// Annull the specified Order.
        /// </summary>
        /// <returns>The annull.</returns>
        /// <param name="Order">Order.</param>
        public Boolean Annull(Models.Order Order)
        {
            return true;
        }

        /// <summary>
        /// Delete the specified Order and Force.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="Order">Order.</param>
        /// <param name="Force">If set to <c>true</c> force.</param>
        public Boolean Delete(Models.Order Order, bool Force = false)
        {
            _db.Orders.Remove(Order);
            return true;
        }

        public void Upload(string slug,Cognitivo.API.Enums.SyncWith SyncWith = Cognitivo.API.Enums.SyncWith.Production)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Order order in _db.Orders.Include(x => x.details).ToList())
            {
                Cognitivo.API.Models.Sales orderModel = new Cognitivo.API.Models.Sales();

                orderModel = UpdateData(orderModel, order);
                syncList.Add(orderModel);
            }

            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Transaction,SyncWith);


        }

        public dynamic UpdateData(Cognitivo.API.Models.Sales Sales, Core.Models.Order sales)
        {
            Sales.cloudId = sales.cloudId;
            Sales.localId = sales.localId;
            // Vat.updatedAt = vat.updatedAt != null ? vat.updatedAt.Value : vat.createdAt.Value;
            Sales.createdAt = sales.createdAt != null ? sales.createdAt.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime();
            Sales.updatedAt = sales.updatedAt != null ? sales.updatedAt.Value.ToUniversalTime() : sales.createdAt.Value.ToUniversalTime();
            Sales.deletedAt = sales.deletedAt != null ? sales.deletedAt.Value.ToUniversalTime() : sales.deletedAt;
            Sales.action = (Cognitivo.API.Enums.Action)sales.action;
            Sales.status = (Cognitivo.API.Enums.Status)sales.status;
            Sales.locationCloudId = sales.location.cloudId;
            Sales.date = sales.date;
            Sales.customerCloudId = sales.customer != null ? sales.customer.cloudId : _db.Contacts.FirstOrDefault().cloudId;
            Sales.paymentContractCloudId = sales.paymentContract != null ? sales.paymentContract.cloudId : null;
            Sales.invoiceNumber = sales.invoiceNumber;
            Sales.InvoiceCode = sales.code;
            Sales.codeExpiry = sales.codeExpiry;
            Sales.currencyCode = sales.currency;
            Sales.rate = sales.currencyRate;
            Sales.interval = sales.interval;

            foreach (OrderDetail detail in sales.details)
            {
                Cognitivo.API.Models.SalesDetail SalesDetail = new Cognitivo.API.Models.SalesDetail();
                SalesDetail.cloudId = detail.cloudId;
                SalesDetail.salesCloudId = detail.order.cloudId;
                SalesDetail.vatCloudId = detail.vat != null ? detail.vat.cloudId : null;
                SalesDetail.itemCloudId = detail.item.cloudId;
                SalesDetail.itemLocalId = detail.item.localId;
                SalesDetail.item = new Cognitivo.API.Models.Item { name = detail.item.name, sku = detail.item.sku };
                SalesDetail.name = detail.item.name;
                SalesDetail.sku = detail.item.sku;
                SalesDetail.cost = detail.cost;
                SalesDetail.quantity = detail.quantity;
                SalesDetail.price = detail.price;
                SalesDetail.updatedAt = detail.updatedAt != null ? detail.updatedAt.Value.ToUniversalTime() : detail.createdAt.Value.ToUniversalTime();
                Sales.details.Add(SalesDetail);
            }


            return Sales;
        }
    }
}
