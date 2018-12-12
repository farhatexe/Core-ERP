using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IQueryable<dynamic> List_IncludeStock(Models.Location location, DateTime? date)
        {
            date = date.HasValue ? date : DateTime.Now;
            
               IQueryable<dynamic> query = from i in db.Items
                                            join movements in 
                                            (from movements in db.ItemMovements where location.localId == movements.localId && movements.date >= date select movements)
                                            on i equals movements.item into itemStock
                                            from Is in itemStock.DefaultIfEmpty()
                                            join branch in db.Locations on Is.location equals branch into locationstock
                                            from j in locationstock.DefaultIfEmpty()
                                            group Is by i into g
                                            select new
                                            {
                                                Item = g.Key,
                                                Balance = g.Key.ItemMovements.Sum(x=>x.credit-x.debit)
                                            };
            return query;
        }

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
