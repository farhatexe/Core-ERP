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

    
    }
}
