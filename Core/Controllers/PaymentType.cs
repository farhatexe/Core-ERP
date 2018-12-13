using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Controllers
{
    public class PaymentType
    {
        private Context _db;
        public PaymentType(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.PaymentType> List()
        {
            _db.PaymentTypes.Load();
            return _db.PaymentTypes.Local.ToObservableCollection();
        }

        public void Add(Models.PaymentType Entity)
        {
            _db.PaymentTypes.Add(Entity);
        }

        public void Delete(Models.PaymentType Entity)
        {
            _db.PaymentTypes.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
