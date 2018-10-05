using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core
{
    public class CustomerRepository
    {
        private Context _db;

        public CustomerRepository(Context db)
        {
            _db = db;
        }
        public ObservableCollection<Contact> LIST()
        {
            _db.Contacts.Load();
            return _db.Contacts.Local.ToObservableCollection();
        }
        public void Add(Contact Entity)
        {
            _db.Contacts.Add(Entity);
        }
        public void Delete(Contact Entity)
        {
            _db.Contacts.Remove(Entity);
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
