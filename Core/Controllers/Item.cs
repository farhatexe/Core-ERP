using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace Core.Controllers
{
    public class ItemController
    {
        private Context db;

        public ItemController(Context DB)
        {
            db = DB;
        }

        public ObservableCollection<Models.Item> List()
        {
            db.Items.Load();
            return db.Items.Local.ToObservableCollection();
        }

        /// <summary>
        /// Itemses the with stock by location.
        /// </summary>
        /// <returns>The with stock by location.</returns>
        /// <param name="locationList">Location list.</param>
        /// <param name="date">Date.</param>
        /// <param name="LocalData">If set to <c>true</c> local data.</param>
        public dynamic ItemsWithStockByLocation(Models.Location location, DateTime? date, bool LocalData = true)
        {
            date = date.HasValue ? date : DateTime.Now;

            //example of left join.
            //var query = from person in people
            //            join pet in pets on person equals pet.Owner into gj
            //            from subpet in gj.DefaultIfEmpty()
            //            select new { person.FirstName, PetName = subpet?.Name ?? String.Empty };

            try
            {
                IEnumerable<object> query = from Item in db.Items
                                            join Movements in db.ItemMovements on Item equals Movements.item into itemStock
                                            from Is in itemStock.DefaultIfEmpty()
                                            group Is by Is.item into g
                                            join Location in db.Locations on g.FirstOrDefault().location  equals location into locationStock
                                            from j in locationStock
                                            where location.localId == g.FirstOrDefault().location.localId 
                                            select new
                                            {
                                                Item = g.Key.localId,
                                                Balance = j.itemMovements.Sum(x => x.credit - x.debit),
                                                Cost=g.Key.cost
                                            };
                return query; // query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

           



            
        }
        /// <summary>
        /// Stocks the by location.
        /// </summary>
        /// <returns>The by location.</returns>
        /// <param name="location">Location.</param>
        /// <param name="LocalData">If set to <c>true</c> local data.</param>
        /// <param name="date">Date.</param>
        public dynamic StockByLocation(Models.Location location, DateTime? date, bool LocalData = true)
        {
            date = !date.HasValue ? DateTime.Now : date;

            //TODO: this is a right join. change into left join to bring items without stock.
            var query = from i in db.Items
                        join im in db.ItemMovements on i equals im.item
                        where im.location == location && im.date >= date
                        group im by im.location into g
                        select new
                        {
                            Item = g.FirstOrDefault().item, //g.FirstOrDefault().Item,
                            Location = g.FirstOrDefault().location,
                            Balance = g.Sum(x => x.credit - x.debit)
                            
                        };

            return query;
        }

        /// <summary>
        /// Stock this instance.
        /// </summary>
        /// <returns>The stock.</returns>
        public IQueryable<object> Stock()
        {
            var query = from i in db.Items
                        join im in db.ItemMovements on i equals im.item
                        group im by im.item into g
                        select new
                        {
                            Item=g.FirstOrDefault().item,
                            Location=g.FirstOrDefault().location,
                            StockLevel = g.Sum(x => (x.credit - x.debit))
                        };

            return query;
        }

        //todo, make same calls but directly from cognitivo servers. This will get shared data.

        /// <summary>
        /// Add the specified Entity.
        /// </summary>
        /// <param name="Entity">Entity.</param>
        public void Add(Models.Item Entity)
        {
            db.Items.Add(Entity);
        }

        /// <summary>
        /// Delete the specified Entity.
        /// </summary>
        /// <param name="Entity">Entity.</param>
        public void Delete(Models.Item Entity)
        {
            db.Items.Remove(Entity);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        /// <summary>
        /// Checks the price.
        /// </summary>
        /// <param name="item">Item.</param>
        public void CheckPrice(Models.Item item)
        {

        }

        /// <summary>
        /// Checks the price on sales.
        /// </summary>
        /// <param name="order">Order.</param>
        public void CheckPriceOnSales(Models.Order order)
        {

        }

        /// <summary>
        /// Checks the stock.
        /// </summary>
        /// <param name="item">Item.</param>
        public void CheckStock(Models.Item item)
        {

        }

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> ItemList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.Item);

            foreach (dynamic data in ItemList)
            {
                Item item = new Core.Models.Item
                {
                    cloudId = data.cloudId,
                    globalId = data.globalItem != null ? (int)data.globalItem : 0,
                    shortDescription = data.shortDescription,
                    longDescription = data.longDescription,
                    action = (Core.Enums.Action)data.action,
                    categoryCloudId = data.categoryCloudId,
                    barCode = data.barCode,
                    cost = data.cost ?? 0,
                    currencyCode = data.currencyCode,
                    price = data.price ?? 0,
                    sku = data.sku,
                    weighWithScale = data.weighWithScale ?? 0,
                    weight = data.weight ?? 0,
                    volume = data.volume,
                    isPrivate = data.isPrivate,
                    isActive = data.isActive,
                };

                db.Items.Add(item);

            }

            db.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Item item in db.Items.ToList())
            {
                item.createdAt = item.createdAt;
                item.updatedAt = item.createdAt;
                syncList.Add(item);
            }

            List<object> ItemList = db.Items.Cast<object>().ToList();
            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Item);

        }
    }
}
