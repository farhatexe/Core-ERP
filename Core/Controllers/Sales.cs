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
        public ObservableCollection<Models.Order> List()
        {
            _db.Orders.Load();
            return _db.Orders.Local.ToObservableCollection();
        }

        /// <summary>
        /// Search the specified Query.
        /// </summary>
        /// <param name="Query">Search Query.</param>
        public void Search(string Query)
        {
            return;
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
        /// Approve the specified Context, Order, RecalculatePrices and IgnoreErrors.
        /// </summary>
        /// <param name="Context">Reference your DbContext</param>
        /// <param name="Order">Order</param>
        /// <param name="RecalculatePrices">If set to <c>true</c> recalculate prices.</param>
        /// <param name="IgnoreErrors">If set to <c>true</c> ignore errors.</param>
        public void Approve(Models.Order Order, bool RecalculatePrices = false, bool IgnoreErrors = false)
        {

            //Validate Stock Levels,
            foreach (var detail in Order.Details.Where(x => x.Item.type == Enums.ItemTypes.Stockable))
            {
                // Check stock levels of each item for that location.
                // TODO: place this code into item controller and re-use code from there. 
                decimal InStock = _db.ItemMovements
                                         .Where(x => x.Location == Order.Location && x.Item == detail.Item)
                                         .Sum(y => y.Debit - y.Credit);

                //If Stock is less than or equal to 0, send message of OutOfStock
                if (InStock <= 0) { detail.Message = Message.Warning.OutOfStock; }
            }

            if (IgnoreErrors == false && Order.Details.Any(x => x.Message == Message.Warning.OutOfStock))
            {
                //If stockable items are not in stock, and IgnoreErrors is set to false; exit code
                return;
            }

            //Insert into Stock Movements
            foreach (var detail in Order.Details.Where(x => x.Item.type == Enums.ItemTypes.Stockable))
            {
                //TODO: take this code to ItemMovement Unit Of Work.
                Models.ItemMovement Movement = new Models.ItemMovement()
                {
                    Item = detail.Item,
                    Date = Order.Date,
                    Credit = 0,
                    Debit = detail.Quantity,
                    Location = Order.Location
                };

                _db.ItemMovements.Add(Movement);
            }

            //Check Prices
            if (RecalculatePrices)
            {
                //TODO: run promotions check again, simply call function.
            }

            //Insert into Schedual
          

            //Change Status
            Order.Status = Enums.Status.Approved;

            //Generate Invoice Number
            if (Order.InvoiceNumber == "")
            {
                Controllers.DocumentController rangeRepository = new Controllers.DocumentController(_db);

                rangeRepository.GenerateInvoiceNumber(Order.Range);
                //run method for invoice generation.
            }

            _db.SaveChanges();
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
