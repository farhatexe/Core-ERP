using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class AccountController
    {
        private Context _db;

        public AccountController(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Account> List()
        {
            _db.Accounts.Load();
            return _db.Accounts.Local.ToObservableCollection();
        }

        public void Add(Account Entity)
        {
            _db.Accounts.Add(Entity);
        }

        public void Delete(Account Entity)
        {
            _db.Accounts.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> AccountList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.Account);
            
            foreach (dynamic data in AccountList)
            {
                Account account = new Core.Models.Account
                {
                    cloudId = data.cloudId,
                    name = data.name,
                    number = data.number,
                    currency = data.currencyCode,

                };
                _db.Accounts.Add(account);

            }
          _db.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> AccountList = _db.Contacts.Cast<object>().ToList();
            CognitivoAPI.UploadData(slug, "", AccountList, Core.API.CognitivoAPI.Modules.Account );

        }

    }
}
