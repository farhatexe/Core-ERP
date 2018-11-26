using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Controllers
{
    public class PaymentContract
    {
        private Context _db;
        public PaymentContract(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.PaymentContract> List()
        {
            _db.PaymentContracts.Load();
            return _db.PaymentContracts.Local.ToObservableCollection();
        }

        public void Add(Models.PaymentContract Entity)
        {
            _db.PaymentContracts.Add(Entity);
        }

        public void Delete(Models.PaymentContract Entity)
        {
            _db.PaymentContracts.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
