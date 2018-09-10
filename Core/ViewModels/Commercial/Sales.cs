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
        List<Models.Order> Orders { get; set; }

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

        public Boolean Save(Models.Order Order)
        {
            return true;
        }

        public Boolean Approve(Models.Order Order)
        {
            return true;
        }

        public Boolean Annull(Models.Order Order)
        {
            return true;
        }

        public Boolean Delete(Models.Order Order, bool Force = false)
        {
            return true;
        }
    }
}
