using Core.Models;
using System;

namespace Core.Controllers
{
    /// <summary>
    /// Inventory.
    /// </summary>
    public class InventoryController
    {
        private Context db;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.Inventory"/> class.
        /// </summary>
        /// <param name="ContextDb">Context db.</param>
        public InventoryController(Context ContextDb)
        {
            db = ContextDb;
        }

        /// <summary>
        /// Add the specified Entity.
        /// </summary>
        /// <param name="Entity">Entity.</param>
        public void Add(Models.Inventory Entity)
        {
            db.Inventories.Add(Entity);
        }

        /// <summary>
        /// Delete the specified Entity.
        /// </summary>
        /// <param name="Entity">Entity.</param>
        public void Delete(Models.Inventory Entity)
        {
            db.Inventories.Remove(Entity);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            db.SaveChanges();
        }


        /// <summary>
        /// Calculates the inventory detail. Will return the stock levels of each product within specified location.
        /// </summary>
        /// <returns>List of Inventory Details.</returns>
        /// <param name="location">Location to filter.</param>
        public Models.Inventory CalculateDetail(Models.Location location)
        {
            //Get List of Items with Inventory
            ItemController itemController = new ItemController(db);
            var ListOfItemsWithStock = itemController.StockByLocation(location);

            Models.Inventory inventory = new Models.Inventory()
            {
                Date = DateTime.Now,
                Location = location
            };

            foreach (dynamic item in ListOfItemsWithStock)
            {
                Models.InventoryDetail detail = new Models.InventoryDetail()
                {
                    Item = item.Item,
                    QtySystem = item.Balance,
                    Cost = item.Cost,
                };

                inventory.Details.Add(detail);
            }

            return inventory;
        }
    }
}
