using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core
{
    public class RangeRepository
    {
        private Context _db;

        public RangeRepository(Context db)
        {
            _db = db;
        }
        public ObservableCollection<Range> LIST()
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

        public string GenerateInvoiceNumber(Range range)
        {
            int current_value = range.CurrentValue;
            int end_value = range.EndingValue;

            string prefix = string.Empty;
            prefix = range.Template;
            prefix = return_Prefix(prefix);

            if (prefix != null & current_value <= end_value)
            {

                //Range
                if (prefix.Contains("#Range"))
                {
                    //Add Padding filler
                    range.CurrentValue += 1;
                    
                    prefix = prefix.Replace("#Range", Prefix);
                }
                else
                {
                    range.CurrentValue += 1;

                }
            }
            return prefix;

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
            if (prefix.Contains("#Now"))
            { prefix = prefix.Replace("#Now", DateTime.Now.Date.ToString()); }

            //Range will be calculated later on, as there is extra business logic to be handled
            return prefix;
        }
    }
}
