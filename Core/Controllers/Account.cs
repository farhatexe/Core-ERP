﻿using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class Account
    {
        private Context _db;

        public Account(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.Account> List()
        {
            _db.Accounts.Load();
            return _db.Accounts.Local.ToObservableCollection();
        }

        public void Add(Models.Account Entity)
        {
            _db.Accounts.Add(Entity);
        }

        public void Delete(Models.Account Entity)
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
                Models.Account account = new Core.Models.Account
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
            List<object> syncList = new List<object>();
            foreach (Core.Models.Account item in _db.Accounts.ToList())
            {
                item.createdAt = item.createdAt.ToUniversalTime();
                item.updatedAt = item.createdAt.ToUniversalTime();
                syncList.Add(item);
            }
            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Account);

        }

    }
}
