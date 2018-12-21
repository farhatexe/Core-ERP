using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class PaymentContract
    {
        private Context _db;
        public PaymentContract(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.PaymentContract> List()
        {
            _db.PaymentContracts.Load();
            return _db.PaymentContracts.Local.ToObservableCollection();
        }

        public void Add(Models.PaymentContract Entity)
        {
            _db.PaymentContracts.Add(Entity);
        }

        public void Delete(Models.PaymentContract Entity)
        {
            _db.PaymentContracts.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

      


        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.PaymentContract vat in _db.PaymentContracts.Include(x => x.details).ToList())
            {
                Cognitivo.API.Models.PaymentContract contractModel = new Cognitivo.API.Models.PaymentContract();

                contractModel = UpdateData(contractModel, vat);
                syncList.Add(contractModel);
            }

            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Contract);

            foreach (dynamic data in ReturnItem)
            {
                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.PaymentContract contract = _db.PaymentContracts.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        contract.updatedAt = Convert.ToDateTime(data.updatedAt);
                        contract.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        contract.cloudId = data.cloudId;
                        contract.updatedAt = Convert.ToDateTime(data.updatedAt);
                        contract.updatedAt = contract.updatedAt.Value.ToLocalTime();
                        contract.createdAt = Convert.ToDateTime(data.createdAt);
                        contract.createdAt = contract.createdAt.Value.ToLocalTime();
                        foreach (var item in data.details)
                        {
                            int localDetailId = (int)data.localId;
                            Models.PaymentContractDetail detail = contract.details.Where(x => x.localId == localDetailId).FirstOrDefault() ?? new PaymentContractDetail();

                            detail.cloudId = item.cloudId;
                            detail.offset = item.coefficient;
                            detail.percentage = item.percent;
                            detail.updatedAt = Convert.ToDateTime(item.updatedAt);
                            detail.updatedAt = detail.updatedAt.Value.ToLocalTime();
                            detail.createdAt = Convert.ToDateTime(item.createdAt);
                            detail.createdAt = detail.createdAt.Value.ToLocalTime();

                            if (detail.localId == 0)
                            {
                                contract.details.Add(detail);
                            }

                        }

                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.PaymentContract contract = new Models.PaymentContract();
                    contract.cloudId = data.cloudId;
                    contract.updatedAt = Convert.ToDateTime(data.updatedAt);
                    contract.updatedAt = contract.updatedAt.Value.ToLocalTime();
                    contract.createdAt = Convert.ToDateTime(data.createdAt);
                    contract.createdAt = contract.createdAt.Value.ToLocalTime();
                    foreach (var item in data.details)
                    {

                        Models.PaymentContractDetail detail = new PaymentContractDetail();

                        detail.cloudId = item.cloudId;
                        detail.offset = item.coefficient;
                        detail.percentage = item.percent;
                        detail.updatedAt = Convert.ToDateTime(item.updatedAt);
                        detail.updatedAt = detail.updatedAt.Value.ToLocalTime();
                        detail.createdAt = Convert.ToDateTime(item.createdAt);
                        detail.createdAt = detail.createdAt.Value.ToLocalTime();
                        contract.details.Add(detail);


                    }
                    _db.PaymentContracts.Add(contract);

                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.PaymentContract contract = _db.PaymentContracts.Where(x => x.localId == localId).FirstOrDefault();
                    contract.updatedAt = Convert.ToDateTime(data.updatedAt);
                    contract.updatedAt = contract.updatedAt.Value.ToLocalTime();
                    contract.createdAt = Convert.ToDateTime(data.createdAt);
                    contract.createdAt = contract.createdAt.Value.ToLocalTime();
                }
            }

        }
        public dynamic UpdateData(Cognitivo.API.Models.PaymentContract Contract, Core.Models.PaymentContract contract)
        {
            Contract.updatedAt = contract.updatedAt != null ? contract.updatedAt.Value : contract.createdAt.Value;
            Contract.action = (Cognitivo.API.Enums.Action)contract.action;
            Contract.name = contract.name;
            foreach (PaymentContractDetail detail in contract.details)
            {
                Cognitivo.API.Models.PaymentContractDetail ContractDetail = new Cognitivo.API.Models.PaymentContractDetail();
                ContractDetail.offset = detail.offset;
                ContractDetail.percent = detail.percentage;
                ContractDetail.updatedAt = detail.updatedAt != null ? detail.updatedAt.Value : detail.createdAt.Value;
                Contract.details.Add(ContractDetail);
            }


            return Contract;
        }
    }
}
