using System;
using System.Collections.Generic;

namespace Core.ViewModels.Inventory
{
    public class Item
    {
        List<Models.Inventories.Item> Items { get; set; }

        public void List()
        {
            return;
        }

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
