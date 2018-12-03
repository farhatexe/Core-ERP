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
        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> TaxList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.Vat);

            foreach (dynamic data in TaxList)
            {
                Vat tax = new Core.Models.Vat
                {
                    cloudId = data.cloudId,
                    name = data.name,
                };
                foreach (var detail in data.details)
                {
                    VatDetail vatdetail = new VatDetail
                    {
                        cloudId = detail.cloudId,
                        name = detail.name,
                        coefficient = detail.coefficient,
                        percentage = detail.percentage
                    };
                    tax.details.Add(vatdetail);
                }

                _db.Vats.Add(tax);

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
