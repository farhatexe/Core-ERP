using Core.API;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Controllers
{
   public class Company
    {
        private Context _db;

        public Company(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.Company> List()
        {
            _db.Companies.Load();
            return _db.Companies.Local.ToObservableCollection();
        }

        public void Add(Models.Company Entity)
        {
            _db.Companies.Add(Entity);
        }

        public void Delete(Models.Company Entity)
        {
            _db.Companies.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Download(string slug, string key)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> CompanyList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Company);

            foreach (dynamic data in CompanyList)
            {
                Models.Company company = new Models.Company();
                company.slugCognitivo = data.slugCognitivo;
                company.name = data.name;
                company.address = data.address;
                company.email = data.email;
                company.taxId = data.taxId;
                _db.Companies.Add(company);
                



            }

            _db.SaveChanges();
        }
    }
}
