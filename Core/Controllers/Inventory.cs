using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Controllers
{
    public class Inventory
    {
        public Inventory()
        {

        }

        /// <summary>
        /// Create creates a list of Inventory based on your <paramref name="location"/>. This function willl return the full list of items from this location.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="Context">Db Context.</param>
        /// <param name="location">Location for Inventory.</param>
        public Models.Inventory Create(ref Models.Context Context, Models.Location location)
        {

            Models.Inventory inventory = new Models.Inventory(){
                Date = DateTime.Now,
                Location = location
            };

            List<Models.Item> items = Context.Items.Where(x => x.Company.Id == location.Company.Id).ToList();

            var itemsWithStock = Context.ItemMovements.Where(x => x.Location.Id == location.Id)
                                                       .GroupBy(y => y.Item)
                                                       .ToList();

            foreach (Models.Item item in items)
            {
                Models.InventoryDetail detail = new Models.InventoryDetail()
                {
                    Item = item,
                    QtySystem = itemsWithStock.Sum(z => z.Where(x => x.Item.Id == item.Id).Sum(y => y.Credit) - z.Where(x => x.Item.Id == item.Id).Sum(y => y.Debit)),
                    QtyCounted = 0,
                    Cost = item.Cost,
                };

                inventory.Details.Add(detail);
            }

            return inventory;
        }
    }
}
