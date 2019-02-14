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



                if (location.localId == 0)
                {
                    _db.Locations.Add(location);
                }


            }
            _db.SaveChanges();
        }
        public void Upload(string slug, Cognitivo.API.Enums.SyncWith SyncWith = Cognitivo.API.Enums.SyncWith.Production)
        {
            Core.Models.Company company = _db.Companies.Where(x => x.slugCognitivo == slug).FirstOrDefault();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();
            foreach (Core.Models.Location item in _db.Locations.ToList())
            {
                Cognitivo.API.Models.Location locationModel = new Cognitivo.API.Models.Location();

                locationModel = UpdateData(locationModel, item);
                syncList.Add(locationModel);
            }
            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Location,SyncWith);
            foreach (dynamic data in ReturnItem)
            {
                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.Location location = _db.Locations.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        location.updatedAt = Convert.ToDateTime(data.updatedAt);
                        location.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        location.cloudId = data.cloudId;
                        location.currencyCode = data.currencyCode;
                        location.address = data.address;
                        location.email = data.email;
                        location.telephone = data.telephone;
                        location.name = data.name;
                        location.updatedAt = Convert.ToDateTime(data.updatedAt);
                        location.updatedAt = location.updatedAt.Value.ToLocalTime();
                        location.createdAt = Convert.ToDateTime(data.createdAt);
                        location.createdAt = location.createdAt.Value.ToLocalTime();
                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.Location location = new Models.Location();
                    location.company = company;
                    location.cloudId = data.cloudId;
                    location.currencyCode = data.currencyCode;
                    location.address = data.address;
                    location.email = data.email;
                    location.telephone = data.telephone;
                    location.name = data.name;
                    location.updatedAt = Convert.ToDateTime(data.updatedAt);
                    location.updatedAt = location.updatedAt.Value.ToLocalTime();
                    location.createdAt = Convert.ToDateTime(data.createdAt);
                    location.createdAt = location.createdAt.Value.ToLocalTime();
                    _db.Locations.Add(location);
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.Location location = _db.Locations.Where(x => x.localId == localId).FirstOrDefault();
                    location.updatedAt = Convert.ToDateTime(data.updatedAt);
                    location.updatedAt = location.updatedAt.Value.ToLocalTime();
                    location.createdAt = Convert.ToDateTime(data.createdAt);
                    location.createdAt = location.createdAt.Value.ToLocalTime();
                }
            }

            _db.SaveChanges();
        }
        public dynamic UpdateData(Cognitivo.API.Models.Location Location, Core.Models.Location location)
        {
            Location.updatedAt = location.updatedAt != null ? location.updatedAt.Value : location.createdAt.Value;
            Location.action = (Cognitivo.API.Enums.Action)location.action;
            Location.cloudId = location.cloudId;
            Location.createdAt = location.createdAt != null ? location.createdAt.Value : DateTime.Now;
            Location.currencyCode = location.currencyCode;
            Location.deletedAt = location.deletedAt != null ? location.deletedAt.Value : location.deletedAt;
            Location.localId = location.localId;
            Location.name = location.name;
            Location.vatCloudId = location.vat!=null ? location.vat.cloudId:0;
            location.address = location.address;
            location.email = location.email;
            location.telephone = location.telephone;

            return Location;
        }
    }

}
