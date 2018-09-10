using System;
using System.Collections.Generic;

namespace Core.ViewModels.Commercial
{
    public class Sales
    {
        /// <summary>
        /// All the Orders brought by the list.
        /// </summary>
        /// <value>Orders (Purchases or Sales)</value>
        List<Models.Orders.Order> Orders { get; set; }

        public void List()
        {
            return;
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
        public Models.Orders.Order New() => new Models.Orders.Order();

        public Boolean Save(Models.Orders.Order Order)
        {
            return true;
        }

        public Boolean Approve(Models.Orders.Order Order)
        {
            return true;
        }

        public Boolean Annull(Models.Orders.Order Order)
        {
            return true;
        }

        public Boolean Delete(Models.Orders.Order Order, bool Force = false)
        {
            return true;
        }
    }
}
