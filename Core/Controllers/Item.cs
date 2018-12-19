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
                                        (from movements in db.ItemMovements where location.localId == movements.location.localId && movements.date <= date select movements)
                                        on i equals movements.item into itemStock
                                        from Is in itemStock.DefaultIfEmpty()
                                        join branch in db.Locations on Is.location equals branch into locationstock
                                        from j in locationstock.DefaultIfEmpty()
                                        group Is by i into g
                                        select new
                                        {
                                            Item = g.Key,
                                            Balance = g.Sum(x => x != null ? (x.debit - x.credit) : 0)
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

        public void Download(string slug, string key)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> ItemList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Item);

            foreach (dynamic data in ItemList)
            {
                int cloudId = (int)data.cloudId;
                Models.Item item = db.Items.Where(x => x.cloudId == cloudId).FirstOrDefault() ?? new Models.Item();
                item.cloudId = data.cloudId;
                item.globalId = data.globalItem != null ? (int)data.globalItem : 0;
                item.shortDescription = data.shortDescription;
                item.longDescription = data.longDescription;
                item.categoryCloudId = data.categoryCloudId;
                item.barCode = data.barCode;
                item.cost = data.cost ?? 0;
                item.currencyCode = data.currencyCode;
                item.price = data.price ?? 0;
                item.sku = data.sku;
                item.name = data.name;
                item.weighWithScale = data.weighWithScale ?? 0;
                item.weight = data.weight ?? 0;
                item.volume = data.volume;
                item.isPrivate = data.isPrivate;
                item.isActive = data.isActive;
                item.updatedAt = Convert.ToDateTime(data.updatedAt);
                item.createdAt = Convert.ToDateTime(data.createdAt);
                item.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;


                if (item.localId == 0)
                {
                    db.Items.Add(item);
                }



            }

            db.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Item item in db.Items.ToList())
            {
                Cognitivo.API.Models.Item Item = new Cognitivo.API.Models.Item();

                Item = Updatedata(Item, item);
                syncList.Add(Item);
            }


            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Item);
            foreach (dynamic data in ReturnItem)
            {

                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.Item item = db.Items.Where(x => x.localId == localId).FirstOrDefault();
                    if (data.deletedAt != null)
                    {
                        item.isActive = false;
                        item.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        item.cloudId = data.cloudId;
                        item.globalId = data.globalItem != null ? (int)data.globalItem : 0;
                        item.shortDescription = data.shortDescription;
                        item.longDescription = data.longDescription;
                        item.categoryCloudId = data.categoryCloudId;
                        item.barCode = data.barCode;
                        item.cost = data.cost ?? 0;
                        item.currencyCode = data.currencyCode;
                        item.price = data.price ?? 0;
                        item.sku = data.sku;
                        item.name = data.name;
                        item.weighWithScale = data.weighWithScale ?? 0;
                        item.weight = data.weight ?? 0;
                        item.volume = data.volume;
                        item.isPrivate = data.isPrivate;
                        item.isActive = data.isActive;
                        item.updatedAt = Convert.ToDateTime(data.updatedAt);
                        item.createdAt = Convert.ToDateTime(data.createdAt);

                    }


                }


            }
            db.SaveChanges();


        }
        public dynamic Updatedata(dynamic Item, Core.Models.Item item)
        {
            Item.updatedAt = item.updatedAt != null ? item.updatedAt.Value : DateTime.Now;
            Item.action = (Cognitivo.API.Enums.Action)item.action;
            Item.barCode = item.barCode;
            Item.categoryCloudId = item.categoryCloudId;
            Item.cloudId = item.cloudId;
            Item.cost = item.cost;
            Item.createdAt = item.createdAt != null ? item.createdAt.Value : DateTime.Now;
            Item.currencyCode = item.currencyCode;
            Item.deletedAt = item.deletedAt != null ? item.deletedAt.Value : item.deletedAt;
            Item.globalItem = item.globalId > 0 ? item.globalId : null;
            Item.isActive = item.isActive;
            Item.isPrivate = item.isPrivate;
            Item.isStockable = item.isStockable;
            Item.localId = item.localId;
            Item.longDescription = item.longDescription;
            Item.name = item.name;
            Item.price = item.price;
            Item.shortDescription = item.shortDescription;
            Item.sku = item.sku;
            Item.vatCloudId = item.vatCloudId;
            Item.volume = item.volume;
            Item.weight = item.weight;
            Item.weighWithScale = item.weighWithScale;
            return Item;
        }
    }
}
