using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Controllers
{

    public class ItemPromotion
    {
        private Context _db;
        public ItemPromotion(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.ItemPromotion> List()
        {
            _db.ItemPromotions.Load();
            return _db.ItemPromotions.Local.ToObservableCollection();
        }

        public void Add(Models.ItemPromotion Entity)
        {
            _db.ItemPromotions.Add(Entity);
        }

        public void Delete(Models.ItemPromotion Entity)
        {
            _db.ItemPromotions.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
