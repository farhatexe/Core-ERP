using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
            List<object> ItemCategoryList = new List<object>();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            ItemCategoryList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.ItemCategory);

            foreach (dynamic data in ItemCategoryList)
            {
                int cloudId = (int)data.cloudId;
                Models.ItemCategory itemcategory = _db.ItemCategories.Where(x => x.cloudId == cloudId).FirstOrDefault() ?? new Models.ItemCategory();


                itemcategory.cloudId = data.cloudId;
                itemcategory.name = data.name;
                itemcategory.group = data.group;

                if (itemcategory.localId == 0)
                {
                    _db.ItemCategories.Add(itemcategory);
                }


            }
            _db.SaveChanges();
        }
        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();
            foreach (Core.Models.ItemCategory item in _db.ItemCategories.ToList())
            {
                Cognitivo.API.Models.ItemCategory Category = new Cognitivo.API.Models.ItemCategory();
                Category = Updatedata(Category, item);
                syncList.Add(item);
            }
            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.ItemCategory);
            foreach (dynamic data in ReturnItem)
            {

                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.ItemCategory itemCategory = _db.ItemCategories.Where(x => x.localId == localId).FirstOrDefault();
                    if (data.deletedAt != null)
                    {
                        itemCategory.updatedAt = Convert.ToDateTime(data.updatedAt);
                        itemCategory.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        itemCategory.updatedAt = Convert.ToDateTime(data.updatedAt);
                        itemCategory.updatedAt = itemCategory.updatedAt.Value.ToLocalTime();
                        itemCategory.createdAt = Convert.ToDateTime(data.createdAt);
                        itemCategory.createdAt = itemCategory.createdAt.Value.ToLocalTime();

                        itemCategory.name = data.name;
                        itemCategory.group = data.group;
                        itemCategory.cloudId = data.cloudId;
                      
                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.ItemCategory itemCategory = new ItemCategory();
                    itemCategory.updatedAt = Convert.ToDateTime(data.updatedAt);
                    itemCategory.updatedAt = itemCategory.updatedAt.Value.ToLocalTime();
                    itemCategory.createdAt = Convert.ToDateTime(data.createdAt);
                    itemCategory.createdAt = itemCategory.createdAt.Value.ToLocalTime();

                    itemCategory.name = data.name;
                    itemCategory.group = data.group;
                    itemCategory.cloudId = data.cloudId;

                    _db.ItemCategories.Add(itemCategory);
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.ItemCategory itemCategory = _db.ItemCategories.Where(x => x.localId == localId).FirstOrDefault();
                    itemCategory.updatedAt = Convert.ToDateTime(data.updatedAt);
                    itemCategory.updatedAt = itemCategory.updatedAt.Value.ToLocalTime();
                    itemCategory.createdAt = Convert.ToDateTime(data.createdAt);
                    itemCategory.createdAt = itemCategory.createdAt.Value.ToLocalTime();
                }
            }
            _db.SaveChanges();
        }

        public dynamic Updatedata(Cognitivo.API.Models.ItemCategory Category, Core.Models.ItemCategory item)
        {
            Category.updatedAt = item.updatedAt != null ? item.updatedAt.Value : item.createdAt.Value;
            Category.action = (Cognitivo.API.Enums.Action)item.action;
            Category.name = item.name;
            Category.group = item.group;
            Category.cloudId = item.cloudId;
            Category.createdAt = item.createdAt != null ? item.updatedAt.Value : item.createdAt.Value; 
            Category.deletedAt = item.deletedAt != null ? item.deletedAt.Value : item.deletedAt;
            Category.localId = item.localId;

            return Category;

        }

    }
}
