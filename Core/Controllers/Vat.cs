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
            _db.Vats.Where(x => x.deletedAt == null).Include(x => x.details).Load();
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

                    if (vatdetail.localId == 0)
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
            Core.Models.Company company = _db.Companies.Where(x => x.slugCognitivo == slug).FirstOrDefault();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Vat vat in _db.Vats.Include(x => x.details).ToList())
            {
                Cognitivo.API.Models.Vat vatModel = new Cognitivo.API.Models.Vat();

                vatModel = UpdateData(vatModel, vat);
                syncList.Add(vatModel);
            }

            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Vat);

            foreach (dynamic data in ReturnItem)
            {
                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.Vat vat = _db.Vats.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        vat.updatedAt = Convert.ToDateTime(data.updatedAt);
                        vat.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        vat.name = data.name;
                        vat.cloudId = data.cloudId;
                        vat.updatedAt = Convert.ToDateTime(data.updatedAt);
                        vat.updatedAt = vat.updatedAt.Value.ToLocalTime();
                        vat.createdAt = Convert.ToDateTime(data.createdAt);
                        vat.createdAt = vat.createdAt.Value.ToLocalTime();

                        foreach (var item in data.details)
                        {
                            int localDetailId = (int)item.cloudId;
                            Models.VatDetail detail = vat.details.Where(x => x.cloudId == localDetailId).FirstOrDefault() ?? new VatDetail();

                            detail.cloudId = item.cloudId;
                            detail.coefficient = item.coefficient;
                            detail.name = item.name;
                            detail.percentage = item.percentage;
                            detail.updatedAt = Convert.ToDateTime(item.updatedAt);
                            detail.updatedAt = detail.updatedAt.Value.ToLocalTime();
                            detail.createdAt = Convert.ToDateTime(item.createdAt);
                            detail.createdAt = detail.createdAt.Value.ToLocalTime();

                            if (detail.localId == 0)
                            {
                                vat.details.Add(detail);
                            }

                        }

                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.Vat vat = new Vat();
                    vat.company = company;
                    vat.cloudId = data.cloudId;
                    vat.name = data.name;
                    vat.updatedAt = Convert.ToDateTime(data.updatedAt);
                    vat.updatedAt = vat.updatedAt.Value.ToLocalTime();
                    vat.createdAt = Convert.ToDateTime(data.createdAt);
                    vat.createdAt = vat.createdAt.Value.ToLocalTime();

                    foreach (var item in data.details)
                    {

                        Models.VatDetail detail = new VatDetail();

                        detail.cloudId = item.cloudId;
                        detail.coefficient = item.coefficient;
                        detail.name = item.name;
                        detail.percentage = item.percentage;
                        detail.updatedAt = Convert.ToDateTime(item.updatedAt);
                        detail.updatedAt = detail.updatedAt.Value.ToLocalTime();
                        detail.createdAt = Convert.ToDateTime(item.createdAt);
                        detail.createdAt = detail.createdAt.Value.ToLocalTime();
                        vat.details.Add(detail);


                    }
                    _db.Vats.Add(vat);

                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.Vat vat = _db.Vats.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        vat.updatedAt = Convert.ToDateTime(data.updatedAt);
                        vat.updatedAt = vat.updatedAt.Value.ToLocalTime();
                        vat.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {

                        vat.cloudId = data.cloudId;
                        vat.updatedAt = Convert.ToDateTime(data.updatedAt);
                        vat.updatedAt = vat.updatedAt.Value.ToLocalTime();
                        vat.createdAt = Convert.ToDateTime(data.createdAt);
                        vat.createdAt = vat.createdAt.Value.ToLocalTime();
                    }

                }
            }
            _db.SaveChanges();
        }

        public dynamic UpdateData(Cognitivo.API.Models.Vat Vat, Core.Models.Vat vat)
        {
            Vat.cloudId = vat.cloudId;
            Vat.localId = vat.localId;
            // Vat.updatedAt = vat.updatedAt != null ? vat.updatedAt.Value : vat.createdAt.Value;
            Vat.createdAt = vat.createdAt != null ? vat.createdAt.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime();
            Vat.updatedAt = vat.updatedAt != null ? vat.updatedAt.Value.ToUniversalTime() : vat.createdAt.Value.ToUniversalTime();
            Vat.deletedAt = vat.deletedAt != null ? vat.deletedAt.Value.ToUniversalTime() : vat.deletedAt;
            Vat.action = (Cognitivo.API.Enums.Action)vat.action;
            Vat.name = vat.name;
            foreach (VatDetail detail in vat.details)
            {
                Cognitivo.API.Models.VatDetail VatDetail = new Cognitivo.API.Models.VatDetail();
                VatDetail.cloudId = detail.cloudId;
                VatDetail.coefficient = detail.coefficient;
                VatDetail.name = detail.name;
                VatDetail.percentage = detail.percentage;
                VatDetail.updatedAt = detail.updatedAt != null ? detail.updatedAt.Value.ToUniversalTime() : detail.createdAt.Value.ToUniversalTime();
                Vat.details.Add(VatDetail);
            }


            return Vat;
        }
    }
}
