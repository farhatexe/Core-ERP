﻿using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class ItemCategoryController
    {
        private Context _db;

        public ItemCategoryController(Context db)
        {
            _db = db;
        }

        public ObservableCollection<ItemCategory> List()
        {
            _db.ItemCategories.Load();
            return _db.ItemCategories.Local.ToObservableCollection();
        }

        public void Add(ItemCategory Entity)
        {
            _db.ItemCategories.Add(Entity);
        }

        public void Delete(ItemCategory Entity)
        {
            _db.ItemCategories.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> ItemCategoryList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.ItemCategory);

            foreach (dynamic data in ItemCategoryList)
            {
                ItemCategory itemcategory = new ItemCategory
                {
                    cloudId = data.cloudId,
                    name = data.name,
                    isGrouping=data.group

                };

                _db.ItemCategories.Add(itemcategory);

            }
            _db.SaveChanges();
        }

    }
}