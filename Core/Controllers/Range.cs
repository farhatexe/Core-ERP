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
            List<object> RangeList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.DocumentRange);

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
            foreach (Core.Models.Range item in _db.Ranges.ToList())
            {
                item.createdAt = item.createdAt;
                item.updatedAt = item.updatedAt;
                syncList.Add(item);
            }
            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.ItemCategory);

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
