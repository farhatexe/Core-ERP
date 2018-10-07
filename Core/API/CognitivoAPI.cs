﻿using System;
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
        }

        public List<object> UploadData(String CompanySlug, string APIKey, List<object> SyncList, Modules Module)
        {
            send = new Cognitivo.API.Upload(APIKey, Cognitivo.API.Enums.SyncWith.Playground);

            if (Module == Modules.Vat)
            {
                SyncList = send.Vats(CompanySlug, SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.Item)
            {
                SyncList = send.Item(CompanySlug, SyncList).OfType<object>().ToList();
            }

            return SyncList;
        }
        public List<object> DowloadData(String CompanySlug, string APIKey, Modules Module)
        {
            List<object> SyncList=null;
            receive = new Cognitivo.API.Download(APIKey, Cognitivo.API.Enums.SyncWith.Playground);
            if (Module == Modules.Vat)
            {
                SyncList = receive.Vat(CompanySlug , Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }
            else if (Module == Modules.Item)
            {
                SyncList = receive.Item(CompanySlug,Cognitivo.API.Enums.TimeSpan.LastMonth).OfType<object>().ToList();
            }

            return SyncList;
        }
    }
}
