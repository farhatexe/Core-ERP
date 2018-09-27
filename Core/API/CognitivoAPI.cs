using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.API
{
    class CognitivoAPI
    {
        Cognitivo.API.Upload send;
        public enum Modules
        {
            Vat = 0,
            Item = 1,
        }
        private List<object> UploadData(String CompanySlug, string APIKey, List<object> SyncList, Modules Module)
        {
            send = new Cognitivo.API.Upload(APIKey, Cognitivo.API.Enums.SyncWith.Playground);
            if (Module == Modules.Vat)
            {
                SyncList = send.Vats(CompanySlug, SyncList).OfType<object>().ToList();
            }
            else if (Module == Modules.Vat)
            {
                SyncList = send.Item(CompanySlug, SyncList).OfType<object>().ToList();
            }

            return SyncList;
        }

       




    }

}
