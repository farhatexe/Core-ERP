using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics.Contracts;

namespace Core.Controllers
{
    public class ItemMovementController
    {
        private Context db;

        public ItemMovementController(Context DB)
        {
            db = DB;
        }

        public ObservableCollection<Models.ItemMovement> List()
        {
            db.ItemMovements.Include(x=>x.Item).Load();
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
                        where im.Date >= date
                        group im by im.Location into g
                        select new
                        {
                            g.FirstOrDefault().Item, //g.FirstOrDefault().Item,
                            g.FirstOrDefault().Location,
                            Balance = g.Sum(x => x.Credit - x.Debit)
                        };

            return query;
        }



    }
}
