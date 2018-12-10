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
                _db.Orders.Take(take).Skip(skip).Load();
            }
            else
            {
                _db.Orders.Where(x => x.isArchived == false)
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
                                            Balance = g.Key.ItemMovements.Sum(x=>x.credit-x.debit),
                                            Cost = g.Key.cost
                                        };

            return query;
        }

        /// <summary>
        /// List this instance.
        /// </summary>
        /// <returns>The list.</returns>
        public Core.Models.Order AddDetail(Core.Models.Order order,dynamic Item)
        {
            Models.OrderDetail OrderDetail = new OrderDetail();
            OrderDetail.order = order;
            OrderDetail.item =Item.Item;
            OrderDetail.quantity = 1;
            OrderDetail.price = OrderDetail.item.price;
            order.details.Add(OrderDetail);
            return order;
        }

        /// <summary>
        /// Search the specified query, take and skip.
        /// </summary>
        /// <returns>The search.</returns>
        /// <param name="query">Query.</param>
        /// <param name="take">Take.</param>
        /// <param name="skip">Skip.</param>
        public ObservableCollection<Models.Order> Search(string query, int take = 25, int skip = 0)
        {
            _db.Orders.Where(x => x.invoiceNumber.Contains(query) || x.customer.alias.Contains(query))
               .Take(take)
               .Skip(skip)
               .Load();
            return _db.Orders.Local.ToObservableCollection();
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
        public Models.Order Approve(Models.Order Order, bool RecalculatePrices = false, bool IgnoreErrors = false)
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
                else if(Order.range.endValue <= Order.range.currentValue)
                {
                    Order.message = Message.Warning.OutOfDocumentRange;
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
                //TODO: run promotions check again, simply call function.
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
            else
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

            //Change Status
            Order.status = Enums.Status.Approved;

            //Generate Invoice Number
            if (Order.invoiceNumber == "")
            {
                Order.invoiceNumber = new Controllers
                    .DocumentController(_db)
                    .GenerateInvoiceNumber(Order.range);
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
    }
}
