using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class DocumentController
    {
        private Context _db;

        public DocumentController(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Range> List()
        {
            _db.Ranges.Load();
            return _db.Ranges.Local.ToObservableCollection();
        }

        public void Add(Range Entity)
        {
            _db.Ranges.Add(Entity);
        }

        public void Delete(Range Entity)
        {
            _db.Ranges.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        //public string GenerateInvoiceNumber(Range range)
        //{
        //    int current_value = range.currentValue;
        //    int end_value = range.endingValue;

        //    string prefix = string.Empty;
        //    prefix = range.template;
        //    prefix = return_Prefix(prefix);

        //    if (prefix != null & current_value <= end_value)
        //    {

        //        //Range
        //        if (prefix.Contains("#Range"))
        //        {
        //            //Add Padding filler
        //            range.currentValue += 1;
                    
        //            prefix = prefix.Replace("#Range", prefix);
        //        }
        //        else
        //        {
        //            range.currentValue += 1;

        //        }
        //    }
        //    return prefix;

        //}

        private static string return_Prefix(string prefix)
        {

            //Year
            if (prefix.Contains("#Year"))
            { prefix = prefix.Replace("#Year", DateTime.Now.Year.ToString()); }

            //Month
            if (prefix.Contains("#Month"))
            { prefix = prefix.Replace("#Month", DateTime.Now.Month.ToString()); }

            //Now
            if (prefix.Contains("#Now"))
            { prefix = prefix.Replace("#Now", DateTime.Now.Date.ToString()); }

            //Range will be calculated later on, as there is extra business logic to be handled
            return prefix;
        }

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> RangeList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.DocumentRange);

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
                item.createdAt = item.createdAt.ToUniversalTime();
                item.updatedAt = item.updatedAt.ToUniversalTime();
                syncList.Add(item);
            }
            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.ItemCategory);

        }

    }
}
