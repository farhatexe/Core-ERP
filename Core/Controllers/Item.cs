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
        public dynamic ItemsWithStockByLocation(List<Models.Location> locationList, DateTime? date, bool LocalData = true)
        {
            date = date.HasValue ? date : DateTime.Now;

            //TODO: this is a right join. change into left join to bring items without stock.
            var query = from i in db.Items
                        join im in db.ItemMovements on i equals im.Item
                        into joined
                        from j in joined.DefaultIfEmpty()
                        where locationList.Contains(j.Location) && j.Date >= date
                        select new
                        {
                            Item = i, //g.FirstOrDefault().Item,
                            j.Location,
                            Balance = joined.Sum(x => (x.Credit - x.Debit))
                        };

            return query;
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
                        join im in db.ItemMovements on i equals im.Item
                        where im.Location == location && im.Date >= date
                        group im by im.Location into g
                        select new
                        {
                            g.FirstOrDefault().Item, //g.FirstOrDefault().Item,
                            g.FirstOrDefault().Location,
                            Balance= g.Sum(x => x.Credit - x.Debit)
                        };

            return query;
        }

        /// <summary>
        /// Stock this instance.
        /// </summary>
        /// <returns>The stock.</returns>
        public dynamic Stock()
        {
            var query = from i in db.Items
                        join im in db.ItemMovements on i equals im.Item
                        group im by im.Item into g
                        select new
                        {
                            g.FirstOrDefault().Item,
                            g.FirstOrDefault().Location,
                            StockLevel = g.Sum(x => (x.Credit - x.Debit))
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
    }
}
