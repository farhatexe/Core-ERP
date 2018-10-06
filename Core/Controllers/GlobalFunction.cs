
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
   public static class GlobalFunction
    {
        public static dynamic FetchInventory(Context context, Location location)
        {
            var ItemsWithStock = context.Items.Select(x => new
            {
                Item = x,
                Name = x.name,
                Balance = x.ItemMovements.Where(l => l.Location == location).Sum(y => y.Credit) - x.ItemMovements.Where(l => l.Location == location).Sum(z => z.Debit),
                Cost = x.cost
            }).ToList();

            return ItemsWithStock;
        }
    }
}
