using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics.Contracts;

namespace Core.Controllers
{
    public class ItemMovement
    {
        private Context db;

        public ItemMovement(Context DB)
        {
            db = DB;
        }

        public ObservableCollection<Models.ItemMovement> List()
        {
            db.ItemMovements.Include(x=>x.item).Load();
            return db.ItemMovements.Local.ToObservableCollection();
        }
        /// <summary>
        /// Stocks the per item.
        /// </summary>
        /// <returns>The List of movements</returns>
        /// <param name="LocalData">If set to <c>true</c> local data.</param>
        /// <param name="date">Date.</param>
        public dynamic StockPerItem( DateTime? date, bool LocalData = true)
        {
            date = !date.HasValue ? DateTime.Now : date;

            //TODO: this is a right join. change into left join to bring items without stock.
            var query = from im in db.ItemMovements
                        where im.date >= date
                        group im by im.location into g
                        select new
                        {
                            g.FirstOrDefault().item, //g.FirstOrDefault().Item,
                            g.FirstOrDefault().location,
                            Balance = g.Sum(x => x.credit - x.debit)
                        };

            return query;
        }



    }
}
