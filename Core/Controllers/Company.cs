using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Controllers
{
   public class Company
    {
        private Context _db;

        public Company(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.Company> List()
        {
            _db.Companies.Load();
            return _db.Companies.Local.ToObservableCollection();
        }

        public void Add(Models.Company Entity)
        {
            _db.Companies.Add(Entity);
        }

        public void Delete(Models.Company Entity)
        {
            _db.Companies.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

    }
}
