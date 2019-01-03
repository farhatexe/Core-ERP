using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core.Controllers
{
    public class RangeController
    {
        private Context _db;

        public RangeController(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Range> List()
        {
            _db.Ranges.Load();
            return _db.Ranges.Local.ToObservableCollection();
        }

        public void Add(Range range)
        {
            _db.Ranges.Add(range);
        }

        public void Delete(Range range)
        {
            _db.Ranges.Remove(range);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public string GenerateInvoiceNumber(Range range)
        {
            int current_value = range.currentValue;
            int end_value = range.endValue;

            string number = return_Prefix(range.document.numberTemplate);

            if (range.document.numberTemplate != null & current_value <= end_value)
            {
                if (range.document.numberTemplate.Contains("#Range"))
                {
                    //Add value to currentValue to increment the range on each use.
                    range.currentValue += 1;

                    //Add padding or mask to allow specific number of values before range.
                    int countPlaces = range.currentValue.ToString().Length;
                    string finalMask = Helper.Truncate(range.document.mask, countPlaces);

                    string finalRange = finalMask + range.currentValue.ToString();

                    //Replace value of Range with final number.
                    number = range.document.numberTemplate.Replace("#Range", finalRange);
                }
                else
                {
                    range.currentValue += 1;
                }
            }

            return number;
        }

        private static string return_Prefix(string prefix)
        {
            //Year
            if (prefix.Contains("#Year"))
            { prefix = prefix.Replace("#Year", DateTime.Now.Year.ToString()); }

            //Month
            if (prefix.Contains("#Month"))
            { prefix = prefix.Replace("#Month", DateTime.Now.Month.ToString()); }

            //Now
            if (prefix.Contains("#Date"))
            { prefix = prefix.Replace("#Date", DateTime.Now.Date.ToString()); }

            return prefix;
        }

        public void Download(string slug, string key)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> RangeList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Document);

            foreach (dynamic data in RangeList)
            {
                Range range = new Range
                {
                    cloudId = data.cloudId,
                    currentValue = data.group,
                    endValue=data.endingValue,
                    code = data.code,
                    expiryDate = data.expiryDate,


                };

                _db.Ranges.Add(range);

            }
            _db.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Document document in _db.Documents.Include(x => x.details).ToList())
            {
                Cognitivo.API.Models.Document documentModel = new Cognitivo.API.Models.Document();

                documentModel = UpdateData(documentModel, document);
                syncList.Add(documentModel);
            }

            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Document);

            foreach (dynamic data in ReturnItem)
            {
                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.Document document = _db.Documents.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        document.updatedAt = Convert.ToDateTime(data.updatedAt);
                        document.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        document.name = data.name;
                        document.cloudId = data.cloudId;
                        document.updatedAt = Convert.ToDateTime(data.updatedAt);
                        document.updatedAt = document.updatedAt.Value.ToLocalTime();
                        document.createdAt = Convert.ToDateTime(data.createdAt);
                        document.createdAt = document.createdAt.Value.ToLocalTime();

                        foreach (var item in data.details)
                        {
                            int localDetailId = (int)item.cloudId;
                            Models.Range detail = document.details.Where(x => x.cloudId == localDetailId).FirstOrDefault() ?? new Range();

                            detail.cloudId = item.cloudId;
                            detail.code = item.code;
                            detail.currentValue = item.currentValue;
                            detail.endValue = item.endValue;
                            detail.expiryDate = item.expiryDate;
                            detail.startDate = item.startDate;
                            detail.document = document;
                            detail.updatedAt = Convert.ToDateTime(item.updatedAt);
                            detail.updatedAt = detail.updatedAt.Value.ToLocalTime();
                            detail.createdAt = Convert.ToDateTime(item.createdAt);
                            detail.createdAt = detail.createdAt.Value.ToLocalTime();

                            if (detail.localId == 0)
                            {
                                document.details.Add(detail);
                            }

                        }

                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.Document document = new Document();
                    document.cloudId = data.cloudId;
                    document.name = data.name;
                    document.updatedAt = Convert.ToDateTime(data.updatedAt);
                    document.updatedAt = document.updatedAt.Value.ToLocalTime();
                    document.createdAt = Convert.ToDateTime(data.createdAt);
                    document.createdAt = document.createdAt.Value.ToLocalTime();

                    foreach (var item in data.details)
                    {

                        Models.Range detail = new Range();

                        detail.cloudId = item.cloudId;
                        detail.code = item.code;
                        detail.currentValue = item.currentValue;
                        detail.endValue = item.endValue;
                        detail.expiryDate = item.expiryDate;
                        detail.startDate = item.startDate;
                        detail.document = document;
                        detail.updatedAt = Convert.ToDateTime(item.updatedAt);
                        detail.updatedAt = detail.updatedAt.Value.ToLocalTime();
                        detail.createdAt = Convert.ToDateTime(item.createdAt);
                        detail.createdAt = detail.createdAt.Value.ToLocalTime();
                        document.details.Add(detail);


                    }
                    _db.Documents.Add(document);

                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.Document document = _db.Documents.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        document.updatedAt = Convert.ToDateTime(data.updatedAt);
                        document.updatedAt = document.updatedAt.Value.ToLocalTime();
                        document.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {

                        document.cloudId = data.cloudId;
                        document.updatedAt = Convert.ToDateTime(data.updatedAt);
                        document.updatedAt = document.updatedAt.Value.ToLocalTime();
                        document.createdAt = Convert.ToDateTime(data.createdAt);
                        document.createdAt = document.createdAt.Value.ToLocalTime();
                    }

                }
            }
            _db.SaveChanges();
        }

        public dynamic UpdateData(Cognitivo.API.Models.Document Document, Core.Models.Document document)
        {
            Document.cloudId = document.cloudId;
            Document.localId = document.localId;
            // Vat.updatedAt = vat.updatedAt != null ? vat.updatedAt.Value : vat.createdAt.Value;
            Document.createdAt = document.createdAt != null ? document.createdAt.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime();
            Document.updatedAt = document.updatedAt != null ? document.updatedAt.Value.ToUniversalTime() : document.createdAt.Value.ToUniversalTime();
            Document.deletedAt = document.deletedAt != null ? document.deletedAt.Value.ToUniversalTime() : document.deletedAt;
            Document.action = (Cognitivo.API.Enums.Action)document.action;
            Document.name = document.name;
            foreach (Range detail in document.details)
            {
                Cognitivo.API.Models.Range Range = new Cognitivo.API.Models.Range();
                Range.cloudId = detail.cloudId;
                Range.code = detail.code;
                Range.currentValue = detail.currentValue;
                Range.expiryDate = detail.expiryDate;
                Range.startDate = detail.startDate;
                Range.document = Document;
                Range.updatedAt = detail.updatedAt != null ? detail.updatedAt.Value.ToUniversalTime() : detail.createdAt.Value.ToUniversalTime();
                Document.details.Add(Range);
            }


            return Document;
        }

        private static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
    }
}
