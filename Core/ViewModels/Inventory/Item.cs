using System;
using System.Collections.Generic;

namespace Core.ViewModels.Inventory
{
    public class Item
    {
        List<Models.Inventories.Item> Items { get; set; }

        /// <summary>
        /// List of Items belonging to company.
        /// </summary>
        public void List()
        {
            return;
        }

        /// <summary>
        /// List of Items belonging to company.
        /// </summary>
        public void Search()
        {
            return;
        }

        public Boolean Save(Models.Inventories.Item Item)
        {
            return true;
        }

        public Boolean Delete(Models.Inventories.Item Item)
        {
            return true;
        }
    }
}
