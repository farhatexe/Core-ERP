using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.ViewModels.Commercial
{
    public class Sales
    {
        /// <summary>
        /// All the Orders brought by the list.
        /// </summary>
        /// <value>Orders (Purchases or Sales)</value>
        List<Models.Order> Orders { get; set; }

        /// <summary>
        /// List this instance.
        /// </summary>
        /// <returns>The list.</returns>
        public List<Models.Order> List()
        {
            return Orders;
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
            return true;
        }

        /// <summary>
        /// Approve the specified Context, Order, RecalculatePrices and IgnoreErrors.
        /// </summary>
        /// <param name="Context">Reference your DbContext</param>
        /// <param name="Order">Order</param>
        /// <param name="RecalculatePrices">If set to <c>true</c> recalculate prices.</param>
        /// <param name="IgnoreErrors">If set to <c>true</c> ignore errors.</param>
        public void Approve(ref Models.Context Context, Models.Order Order, bool RecalculatePrices = false, bool IgnoreErrors = false)
        {

            //Validate Stock Levels,
            foreach (var detail in Order.Details.Where(x => x.Item.Type == Models.Item.Types.Stockable))
            {
                decimal InStock = Context.ItemMovements
                                         .Where(x => x.Location == Order.Location && x.Item == detail.Item)
                                         .Sum(y => y.Debit - y.Credit);
                if (InStock <= 0)
                { detail.ErrorMessage = Models.OrderDetail.ErrorMessages.OutOfStock; }
            }

            if (IgnoreErrors == false && Order.Details.Where(x => x.ErrorMessage == Models.OrderDetail.ErrorMessages.OutOfStock).Any())
            {
                //If IngoreErrors is set to false, and items are not InStock, then return without finishing work.
                return;
            }

            //Insert into Stock
            foreach (var detail in Order.Details.Where(x => x.Item.Type == Models.Item.Types.Stockable))
            {
                Models.ItemMovement Movement = new Models.ItemMovement()
                {
                    Item = detail.Item,
                    Date = Order.Date,
                    Debit = detail.Quantity,
                    Location = Order.Location,
                };

                Context.ItemMovements.Add(Movement);
            }

            //Check Prices
            if (RecalculatePrices)
            {
                //TODO: run promotions check again, simply call function.
            }

            //Insert into Schedual

            //Change Status
            Order.Status = Models.Order.Statuses.Approved;

            //Generate Invoice Number
            if (Order.InvoiceNumber == '')
            {
                //run method for invoice generation.
            }

            Context.SaveChanges();
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
            return true;
        }
    }
}
