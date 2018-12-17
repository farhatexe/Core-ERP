using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public void Download(string slug, string key)
        {
            List<object> LocationList = new List<object>();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            LocationList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Location);

            foreach (dynamic data in LocationList)
            {
                int cloudId = (int)data.cloudId;
                Core.Models.Location location = _db.Locations.Where(x => x.cloudId == cloudId).FirstOrDefault() ?? new Core.Models.Location();

                location.cloudId = data.cloudId;
                location.name = data.name;
                location.address = data.address;
                location.telephone = data.telephone;
                location.email = data.email;
                location.currencyCode = data.currencyCode;



                if (location.localId==0)
                {
                    _db.Locations.Add(location);
                }
               

            }
            _db.SaveChanges();
        }
        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();
            foreach (Core.Models.Location item in _db.Locations.ToList())
            {
                item.createdAt = item.createdAt;
                item.updatedAt = item.createdAt;
                syncList.Add(item);
            }
            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Location);

        }
    }

}
