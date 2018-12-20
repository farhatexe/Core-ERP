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
            _db.Vats.Include(x => x.details).Load();
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
        public void Download(string slug, string key)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> TaxList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Vat);

            foreach (dynamic data in TaxList)
            {
                int cloudId = (int)data.cloudId;
                Core.Models.Vat tax = _db.Vats.Where(x => x.cloudId == cloudId).FirstOrDefault() ??
                    new Core.Models.Vat();
                tax.cloudId = data.cloudId;
                tax.name = data.name;

                foreach (var detail in data.details)
                {
                    int cloudDetailId = (int)data.cloudId;
                    Core.Models.VatDetail vatdetail = _db.VatDetails.Where(x => x.cloudId == cloudDetailId).FirstOrDefault() ??
                        new Core.Models.VatDetail();

                    vatdetail.cloudId = detail.cloudId;
                    vatdetail.name = detail.name;
                    vatdetail.coefficient = detail.coefficient;
                    vatdetail.percentage = detail.percentage;
                    vatdetail.updatedAt = Convert.ToDateTime(data.updatedAt);
                    vatdetail.createdAt = Convert.ToDateTime(data.createdAt);
                    vatdetail.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;

                    if (vatdetail.localId==0)
                    {
                        tax.details.Add(vatdetail);
                    }
                  
                }
                if (tax.localId == 0)
                {
                    _db.Vats.Add(tax);
                }
                

            }
            _db.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Vat vat in _db.Vats.ToList())
            {
                vat.createdAt = vat.createdAt;
                vat.updatedAt = vat.updatedAt;
                syncList.Add(vat);
            }

            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Vat);

        }
    }
}
