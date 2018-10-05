﻿using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core
{
    public class Item
    {
        private Context _db;

        public Item(Context db)
        {
            _db = db;
        }
        public ObservableCollection<Models.Item> LIST()
        {
            _db.Items.Load();
            return _db.Items.Local.ToObservableCollection();
        }
        public void Add(Models.Item Entity)
        {
            _db.Items.Add(Entity);
        }
        public void Delete(Models.Item Entity)
        {
            _db.Items.Remove(Entity);
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
