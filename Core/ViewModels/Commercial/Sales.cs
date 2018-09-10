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
        /// Approve the specified Order.
        /// </summary>
        /// <returns>The approve.</returns>
        /// <param name="Order">Order.</param>
        public Boolean Approve(Models.Order Order)
        {
            return true;
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
