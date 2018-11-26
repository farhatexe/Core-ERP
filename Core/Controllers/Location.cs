using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Controllers
{
   

        public class Location
        {
            private Context _db;
            public Location(Context db)
            {
                _db = db;
            }

            public ObservableCollection<Models.Location> List()
            {
                _db.Locations.Load();
                return _db.Locations.Local.ToObservableCollection();
            }

            public void Add(Models.Location Entity)
            {
                _db.Locations.Add(Entity);
            }

            public void Delete(Models.Location Entity)
            {
                _db.Locations.Remove(Entity);
            }

            public void SaveChanges()
            {
                _db.SaveChanges();
            }
        }
   
}
