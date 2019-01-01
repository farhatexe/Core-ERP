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
            _db.PointOfSales.Load();
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
        public void Download(string slug, string key)
        {
            List<object> PointOfSaleList = new List<object>();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            PointOfSaleList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Location);

            foreach (dynamic data in PointOfSaleList)
            {
                int cloudId = (int)data.cloudId;
                Core.Models.PointOfSale pointofsale = _db.PointOfSales.Where(x => x.cloudId == cloudId).Include(x=>x.location).FirstOrDefault() ?? new Core.Models.PointOfSale();

                pointofsale.cloudId = data.cloudId;
                pointofsale.name = data.name;
                


                if (pointofsale.localId == 0)
                {
                    _db.PointOfSales.Add(pointofsale);
                }


            }
            _db.SaveChanges();
        }
        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();
            foreach (Core.Models.PointOfSale item in _db.PointOfSales.Include(x=>x.location).ToList())
            {
                Cognitivo.API.Models.PointOfSale pointofsaleModel = new Cognitivo.API.Models.PointOfSale();

                pointofsaleModel = UpdateData(pointofsaleModel, item);
                syncList.Add(pointofsaleModel);
            }
            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.PointOfSale);
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
                    Models.PointOfSale pointofsale = new Models.PointOfSale();
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
