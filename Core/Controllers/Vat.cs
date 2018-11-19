using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class VatController
    {
        private Context _db;

        public VatController(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Vat> List()
        {
            _db.Vats.Include(x=>x.Details).Load();
            return _db.Vats.Local.ToObservableCollection();
        }

        public void Add(Vat Entity)
        {
            _db.Vats.Add(Entity);
        }

        public void Delete(Vat Entity)
        {
            _db.Vats.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

    
    }
}
