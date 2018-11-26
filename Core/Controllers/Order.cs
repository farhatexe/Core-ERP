using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Controllers
{
    public class Order
    {
        private Context _db;
        public Order(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.Order> List()
        {
            _db.Orders.Load();
            return _db.Orders.Local.ToObservableCollection();
        }

        public void Add(Models.Order Entity)
        {
            _db.Orders.Add(Entity);
        }

        public void Delete(Models.Order Entity)
        {
            _db.Orders.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
