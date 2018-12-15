using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public void Download(string slug, string key)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> ItemCategoryList = CognitivoAPI.DowloadData(slug,key, Core.API.CognitivoAPI.Modules.ItemCategory);

            foreach (dynamic data in ItemCategoryList)
            {
                ItemCategory itemcategory = new ItemCategory
                {
                    cloudId = data.cloudId,
                    name = data.name,
                    group=data.group

                };

                _db.ItemCategories.Add(itemcategory);

            }
            _db.SaveChanges();
        }
        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();
            foreach (Core.Models.ItemCategory item in _db.ItemCategories.ToList())
            {
                item.createdAt = item.createdAt;
                item.updatedAt = item.createdAt;
                syncList.Add(item);
            }
            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.ItemCategory);

        }

    }
}
