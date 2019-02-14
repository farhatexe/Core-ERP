using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{


    public class PointOfSales
    {
        private Context _db;
        public PointOfSales(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.PointOfSale> List()
        {
            _db.PointOfSales.Include(x=>x.defaultAccount).Load();
            //_db.Accounts.Load();
            //_db.PaymentTypes.Load();
            //_db.Companies.Load();
            return _db.PointOfSales.Local.ToObservableCollection();
        }

        public void Add(Models.PointOfSale Entity)
        {
            _db.PointOfSales.Add(Entity);
        }

        public void Delete(Models.PointOfSale Entity)
        {
            _db.PointOfSales.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
        public List<Models.PointOfSale> Download(string slug, string key)
        {
            Core.Models.Company company = _db.Companies.Where(x => x.slugCognitivo == slug).FirstOrDefault();
            List<Models.PointOfSale> pointOfSales = new List<PointOfSale>();
            List<object> PointOfSaleList = new List<object>();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            PointOfSaleList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.PointOfSale);

            foreach (dynamic data in PointOfSaleList)
            {
                int locationid = data.locationCloudId;
                Models.PointOfSale pointofsale = new Models.PointOfSale();
                pointofsale.company = company;
                pointofsale.location = _db.Locations.Where(x => x.cloudId == locationid).FirstOrDefault();
                pointofsale.cloudId = data.cloudId;
                pointofsale.name = data.name;
                pointofsale.updatedAt = Convert.ToDateTime(data.updatedAt);
                pointofsale.updatedAt = pointofsale.updatedAt.Value.ToLocalTime();
                pointofsale.createdAt = Convert.ToDateTime(data.createdAt);
                pointofsale.createdAt = pointofsale.createdAt.Value.ToLocalTime();
                PointOfSaleList.Add(pointofsale);

            }
            return pointOfSales;
          //  _db.SaveChanges();
        }
        public void Upload(string slug, Cognitivo.API.Enums.SyncWith SyncWith = Cognitivo.API.Enums.SyncWith.Production)
        {
            List<Models.PointOfSale> pointOfSales = new List<PointOfSale>();
            Core.Models.Company company = _db.Companies.Where(x => x.slugCognitivo == slug).FirstOrDefault();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();
            foreach (Core.Models.PointOfSale item in _db.PointOfSales.Include(x=>x.location).ToList())
            {
                Cognitivo.API.Models.PointOfSale pointofsaleModel = new Cognitivo.API.Models.PointOfSale();

                pointofsaleModel = UpdateData(pointofsaleModel, item);
                syncList.Add(pointofsaleModel);
            }
            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.PointOfSale,SyncWith);
            foreach (dynamic data in ReturnItem)
            {
                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.PointOfSale pointofsale = _db.PointOfSales.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        pointofsale.updatedAt = Convert.ToDateTime(data.updatedAt);
                        pointofsale.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        pointofsale.cloudId = data.cloudId;
                        pointofsale.name = data.name;
                        pointofsale.updatedAt = Convert.ToDateTime(data.updatedAt);
                        pointofsale.updatedAt = pointofsale.updatedAt.Value.ToLocalTime();
                        pointofsale.createdAt = Convert.ToDateTime(data.createdAt);
                        pointofsale.createdAt = pointofsale.createdAt.Value.ToLocalTime();
                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    int locationid = data.locationCloudId;
                    Models.PointOfSale pointofsale = new Models.PointOfSale();
                    pointofsale.company = company;
                    pointofsale.location = _db.Locations.Where(x => x.cloudId == locationid).FirstOrDefault();
                    pointofsale.defaultAccount = _db.Accounts.FirstOrDefault();
                    pointofsale.cloudId = data.cloudId;
                    pointofsale.name = data.name;
                    pointofsale.updatedAt = Convert.ToDateTime(data.updatedAt);
                    pointofsale.updatedAt = pointofsale.updatedAt.Value.ToLocalTime();
                    pointofsale.createdAt = Convert.ToDateTime(data.createdAt);
                    pointofsale.createdAt = pointofsale.createdAt.Value.ToLocalTime();
                    _db.PointOfSales.Add(pointofsale);
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.PointOfSale pointofsale = _db.PointOfSales.Where(x => x.localId == localId).FirstOrDefault();
                    pointofsale.updatedAt = Convert.ToDateTime(data.updatedAt);
                    pointofsale.updatedAt = pointofsale.updatedAt.Value.ToLocalTime();
                    pointofsale.createdAt = Convert.ToDateTime(data.createdAt);
                    pointofsale.createdAt = pointofsale.createdAt.Value.ToLocalTime();
                }
            }

            _db.SaveChanges();
           
        }
        public dynamic UpdateData(Cognitivo.API.Models.PointOfSale PoinOfSale, Core.Models.PointOfSale pointofsale)
        {
            PoinOfSale.updatedAt = pointofsale.updatedAt != null ? pointofsale.updatedAt.Value : pointofsale.createdAt.Value;
            PoinOfSale.action = (Cognitivo.API.Enums.Action)pointofsale.action;
            PoinOfSale.cloudId = pointofsale.cloudId;
            PoinOfSale.createdAt = pointofsale.createdAt != null ? pointofsale.createdAt.Value : DateTime.Now;
            PoinOfSale.deletedAt = pointofsale.deletedAt != null ? pointofsale.deletedAt.Value : pointofsale.deletedAt;
            PoinOfSale.localId = pointofsale.localId;
            PoinOfSale.name = pointofsale.name;
            PoinOfSale.locationCloudId = pointofsale.location!=null ? pointofsale.location.cloudId:0;
          

            return PoinOfSale;
        }
    }

}
