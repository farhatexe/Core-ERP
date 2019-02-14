using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class PaymentType
    {
        private Context _db;
        public PaymentType(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.PaymentType> List()
        {
            _db.PaymentTypes.Load();
            return _db.PaymentTypes.Local.ToObservableCollection();
        }

        public void Add(Models.PaymentType Entity)
        {
            _db.PaymentTypes.Add(Entity);
        }

        public void Delete(Models.PaymentType Entity)
        {
            _db.PaymentTypes.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
        public void Upload(string slug, Cognitivo.API.Enums.SyncWith SyncWith = Cognitivo.API.Enums.SyncWith.Production)
        {
            Core.Models.Company company = _db.Companies.Where(x => x.slugCognitivo == slug).FirstOrDefault();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.PaymentType paymenttype in _db.PaymentTypes.ToList())
            {
                Cognitivo.API.Models.PaymentType paymenttypeModel = new Cognitivo.API.Models.PaymentType();

                paymenttypeModel = UpdateData(paymenttypeModel, paymenttype);
                syncList.Add(paymenttypeModel);
            }

            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList,API.CognitivoAPI.Modules.PaymentType,  SyncWith);

            foreach (dynamic data in ReturnItem)
            {
                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.PaymentType paymnettype = _db.PaymentTypes.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        paymnettype.updatedAt = Convert.ToDateTime(data.updatedAt);
                        paymnettype.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        paymnettype.cloudId = data.cloudId;
                        paymnettype.name = data.name;
                        paymnettype.updatedAt = Convert.ToDateTime(data.updatedAt);
                        paymnettype.updatedAt = paymnettype.updatedAt.Value.ToLocalTime();
                        paymnettype.createdAt = Convert.ToDateTime(data.createdAt);
                        paymnettype.createdAt = paymnettype.createdAt.Value.ToLocalTime();
                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.PaymentType paymnettype = new Models.PaymentType();
                    paymnettype.company = company;
                    paymnettype.cloudId = data.cloudId;
                    paymnettype.name = data.name;
                    paymnettype.updatedAt = Convert.ToDateTime(data.updatedAt);
                    paymnettype.updatedAt = paymnettype.updatedAt.Value.ToLocalTime();
                    paymnettype.createdAt = Convert.ToDateTime(data.createdAt);
                    paymnettype.createdAt = paymnettype.createdAt.Value.ToLocalTime();

                    _db.PaymentTypes.Add(paymnettype);
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.PaymentType paymnettype = _db.PaymentTypes.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        paymnettype.updatedAt = Convert.ToDateTime(data.updatedAt);
                        paymnettype.updatedAt = paymnettype.updatedAt.Value.ToLocalTime();
                        paymnettype.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {

                        paymnettype.cloudId = data.cloudId;
                        paymnettype.updatedAt = Convert.ToDateTime(data.updatedAt);
                        paymnettype.updatedAt = paymnettype.updatedAt.Value.ToLocalTime();
                        paymnettype.createdAt = Convert.ToDateTime(data.createdAt);
                        paymnettype.createdAt = paymnettype.createdAt.Value.ToLocalTime();
                    }
                }
            }

            _db.SaveChanges();
        }
        public dynamic UpdateData(Cognitivo.API.Models.PaymentType paymenttype, Core.Models.PaymentType item)
        {
            paymenttype.updatedAt = item.updatedAt != null ? item.updatedAt.Value.ToUniversalTime() : item.createdAt.Value.ToUniversalTime();
            paymenttype.action = (Cognitivo.API.Enums.Action)item.action;
            paymenttype.country = item.country;
            paymenttype.cloudId = item.cloudId;
            paymenttype.localId = item.localId;
            paymenttype.name = item.name;
            paymenttype.Behaviour = (Cognitivo.API.Models.PaymentType.Behaviours)item.behavior;
            return paymenttype;
        }
    }
}
