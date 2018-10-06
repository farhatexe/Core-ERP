using Core.Controllers;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Inventory
    {
        private Context Context;

        public Inventory(Context db)
        {
            Context = db;
        }

        public void Add(Models.Inventory Entity)
        {
            Context.Inventories.Add(Entity);
        }
        public void Delete(Models.Inventory Entity)
        {
            Context.Inventories.Remove(Entity);
        }
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Create creates a list of Inventory based on your <paramref name="location"/>. This function willl return the full list of items from this location.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="Context">Db Context.</param>
        /// <param name="location">Location for Inventory.</param>
        public Models.Inventory Create( Models.Location location)
        {

            Models.Inventory inventory = new Models.Inventory()
            {
                Date = DateTime.Now,
                Location = location
            };

            //List<Models.Item> items = _db.Items.Where(x => x.companyId == location.Company.Id).ToList();


            dynamic ItemsWithStock = GlobalFunction.FetchInventory(Context, location);



            foreach (dynamic stock in ItemsWithStock)
            {
                Models.InventoryDetail detail = new Models.InventoryDetail()
                {
                    Item = stock.Item,
                    QtySystem =stock.Balance,
                    QtyCounted = 0,
                    Cost = stock.Cost,
                };

                inventory.Details.Add(detail);
            }

            return inventory;
        }
    }
}
