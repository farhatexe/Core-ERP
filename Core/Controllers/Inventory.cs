using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        public ObservableCollection<Models.Inventory> Calculate(Models.Location location)
        {
            //Get List of Items with Inventory
            ItemController itemController = new ItemController(db);
            var ItemsWithStockByLocation = itemController.ItemsWithStockByLocation(location, DateTime.Now);

            ObservableCollection<Models.Inventory> inventories = new ObservableCollection<Inventory>();

            foreach (dynamic item in ItemsWithStockByLocation)
            {
                Models.Inventory inventory = new Models.Inventory()
                {
                    date = DateTime.Now,
                    location = location,
                    item = item.Item,

                    systemQuantity = item.Balance,
                    cost = item.Cost,
                };

                db.Inventories.Add(inventory);
                inventories.Add(inventory);
            }

            return inventories;
        }

        /// <summary>
        /// Approve the specified Inventory and BulkSave.
        /// </summary>
        /// <param name="Inventory">Individual Inventory Row</param>
        /// <param name="BulkSave">
        ///  If set to <c>true</c>, 
        ///  BulkSave will not save and will expect you to perform a InventoryController.db.SaveChanges() after looping through each Inventory
        /// </param>
        public void Approve(Models.Inventory Inventory, bool BulkSave = false)
        {
            if (Inventory.actualQuantity != null)
            {
                decimal delta = Convert.ToDecimal(Inventory.actualQuantity) - Inventory.systemQuantity;

                Models.ItemMovement movements = new Models.ItemMovement()
                {
                    item = Inventory.item,
                    location = Inventory.location,
                    debit = delta > 0 ? delta : 0,
                    credit = delta < 0 ? delta : 0,
                    date = Inventory.date,
                    comment = "Inventory: " + Inventory.comment
                };

                db.ItemMovements.Add(movements);

                if (BulkSave == false)
                {
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Anull the specified Inventory and BulkSave.
        /// </summary>
        /// <param name="Inventory">Inventory.</param>
        /// <param name="BulkSave">If set to <c>true</c> bulk save.</param>
        public void Anull(Models.Inventory Inventory, bool BulkSave)
        {
            if (Inventory.actualQuantity != null)
            {
                decimal delta = Convert.ToDecimal(Inventory.actualQuantity) - Inventory.systemQuantity;

                Models.ItemMovement movements = new Models.ItemMovement()
                {
                    item = Inventory.item,
                    location = Inventory.location,
                    debit = delta < 0 ? delta : 0,
                    credit = delta > 0 ? delta : 0,
                    date = Inventory.date,
                    comment = "Inventory Revert: " + Inventory.comment
                };

                db.ItemMovements.Add(movements);

                if (BulkSave == false)
                {
                    db.SaveChanges();
                }
            }
        }

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> InventoryList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.Inventory);

            foreach (dynamic data in InventoryList)
            {
                Inventory inventory = new Inventory
                {
                    cloudId = data.cloudId,
                    cost = data.cost,
                    systemQuantity = data.quantiy_system,
                    actualQuantity = data.quantiy_actual,
                    comment = data.comment,


                };

                db.Inventories.Add(inventory);

            }
            db.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> InventoryList = db.Inventories.Cast<object>().ToList();
            CognitivoAPI.UploadData(slug, "", InventoryList, Core.API.CognitivoAPI.Modules.Inventory);

        }
    }
}
