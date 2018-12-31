using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.API
{
   public class CognitivoAPI
    {
        Cognitivo.API.Upload send;
        Cognitivo.API.Download receive;

        public enum Modules
        {
            Vat = 0,
            Item = 1,
            Customer=2,
            Account=3,
            ItemCategory=4,
            DocumentRange=5,
            Inventory=6,
            Location=7,
            Contract=8,
            PaymentType=9


        }

        public List<object> UploadData(String CompanySlug, string APIKey, List<object> SyncList, Modules Module)
        {
            send = new Cognitivo.API.Upload(APIKey, Cognitivo.API.Enums.SyncWith.Local);

            if (Module == Modules.Vat)
            {
                SyncList = send.Vats(CompanySlug, SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.Item)
            {
                SyncList = send.Item(CompanySlug, SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.Customer)
            {
                SyncList = send.Customer(CompanySlug, SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.Account)
            {
                SyncList = send.Account(CompanySlug,SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.DocumentRange)
            {
                SyncList = send.Ranges(CompanySlug, SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.Location)
            {
                SyncList = send.Locations(CompanySlug, SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.PaymentType)
            {
                SyncList = send.PaymentType(CompanySlug,SyncList).OfType<object>().ToList();
            }
            //else if (Module == Modules.Inventory)
            //{
            //    SyncList = send.Inventory(CompanySlug, Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            //}
            return SyncList;
        }
        public List<object> DowloadData(String CompanySlug, string APIKey, Modules Module)
        {
            List<object> SyncList=null;
            receive = new Cognitivo.API.Download(APIKey, Cognitivo.API.Enums.SyncWith.Local);
            if (Module == Modules.Vat)
            {
                SyncList = receive.Vat(CompanySlug , Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();

            }
            else if (Module == Modules.Item)
            {
                SyncList = receive.Item(CompanySlug,Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }
            else if (Module == Modules.Customer)
            {
                SyncList = receive.Customer(CompanySlug, Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }
            else if (Module == Modules.Account)
            {
                SyncList = receive.Account(CompanySlug, Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }
            else if (Module == Modules.DocumentRange)
            {
                SyncList = receive.Range(CompanySlug, Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }
            else if (Module == Modules.Inventory)
            {
                SyncList = receive.Inventory(CompanySlug, Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }
            else if (Module == Modules.ItemCategory)
            {
                SyncList = receive.ItemCategory(CompanySlug, Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }
            else if (Module == Modules.Location)
            {
                SyncList = receive.Location(CompanySlug, Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }

            return SyncList;
        }
    }
}
