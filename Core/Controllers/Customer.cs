using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using System.Collections.ObjectModel;

namespace Core.Controllers
{
    public class CustomerController
    {

        private Context ctx;

        public CustomerController(Context db)
        {
            ctx = db;
        }

        /// <summary>
        /// Gets or sets the customers.
        /// </summary>
        /// <value>The customers.</value>
        IQueryable<Models.Contact> Customers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.ViewModels.Commercial.Customer"/> class.
        /// </summary>
        public ObservableCollection<Contact> LIST(int Take = 25, int Skip = 0)
        {
            Customers = ctx.Contacts.Where(x=>x.deletedAt==null)
                               .Take(Take).Skip(Skip);

            return new ObservableCollection<Contact>(Customers);

        }

        /// <summary>
        /// Search the specified Query.
        /// </summary>
        /// <param name="Query">Query.</param>
        public ObservableCollection<Contact> Search(string Query)
        {

            Customers = ctx.Contacts
                           .Where(x =>(
                                      x.alias.Contains(Query) ||
                                      x.taxId.Contains(Query))
                                 );
            return new ObservableCollection<Contact>(Customers);

        }

        /// <summary>
        /// Filters the Contacts list against local data.
        /// </summary>
        /// <param name="Query">Query.</param>
        public ObservableCollection<Contact> Filter(string Query)
        {
            Customers = ctx.Contacts
            .Where(x =>
                   x.alias.Contains(Query) ||
                   x.taxId.Contains(Query) ||
                   x.telephone.Contains(Query) ||
                   x.email.Contains(Query) ||
                   x.address.Contains(Query)
                   );

            return new ObservableCollection<Contact>(Customers);
        }

        /// <summary>
        /// Gets the last five sales made by this customer.
        /// </summary>
        /// <returns>The last five sales.</returns>
        /// <param name="Contact">Customer (aka Contact).</param>
        public List<Models.Order> GetLastFiveSales(Models.Contact Contact, int Take = 25, int Skip = 0)
        {
            return ctx.Orders.Where(x => x.customer.localId == Contact.localId)
                                 .Include(y => y.details)
                                 .Include(z => z.customer)
                                 .Take(Take)
                                 .Skip(Skip).ToList();
        }

        /// <summary>
        /// Save the specified Contact.
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="Contact">Contact.</param>
        public Models.Contact Save(Models.Contact Contact)
        {
            ctx.SaveChanges();
            return Contact;
        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }

        /// <summary>
        /// Delete the specified Contact.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="Contact">Contact.</param>
        public bool Delete(Models.Contact Contact)
        {
            ctx.Entry<Core.Models.Contact>(Contact).State = EntityState.Deleted;
            return true;
        }

        /// <summary>
        /// Add the specified Entity.
        /// </summary>
        /// <param name="Entity">Entity.</param>
        public void Add(Models.Contact Entity)
        {
            ctx.Contacts.Add(Entity);
        }

        public void Download(string slug,string key)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> CustomerList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Customer);

            foreach (dynamic data in CustomerList)
            {
                int cloudId = (int)data.cloudId;
                Core.Models.Contact contact = ctx.Contacts.Where(x => x.cloudId == cloudId).FirstOrDefault() ?? 
                    new Core.Models.Contact();
                
                contact.cloudId = data.cloudId;
                contact.alias = data.alias;
                contact.taxId = data.taxId;
                contact.address = data.address;
                contact.email = data.email;
                contact.telephone = data.telephone;
                contact.leadTime = data.leadTime;
                contact.creditLimit = data.creditLimit != null ? (int)data.creditLimit : 0;
                contact.updatedAt = Convert.ToDateTime(data.updatedAt);
                contact.createdAt = Convert.ToDateTime(data.createdAt);
                contact.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                if (contact.localId==0)
                {
                    ctx.Contacts.Add(contact);
                }
              

            }
            ctx.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.Models.Company company = ctx.Companies.Where(x => x.slugCognitivo == slug).FirstOrDefault();
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Contact item in ctx.Contacts.ToList())
            {
                Cognitivo.API.Models.Customer Customer = new Cognitivo.API.Models.Customer();
                Customer = Updatedata(Customer, item);
                syncList.Add(Customer);
            }

            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Customer);
            foreach (dynamic data in ReturnItem)
            {

                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.Contact item = ctx.Contacts.Where(x => x.localId == localId).FirstOrDefault();
                    if (data.deletedAt != null)
                    {
                        item.updatedAt = Convert.ToDateTime(data.updatedAt);
                        item.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        item.updatedAt = Convert.ToDateTime(data.updatedAt);
                        item.updatedAt = item.updatedAt.Value.ToLocalTime();
                        item.createdAt = Convert.ToDateTime(data.createdAt);
                        item.createdAt = item.createdAt.Value.ToLocalTime();

                        item.address = data.address;
                        item.alias = data.alias;
                        item.cloudId = data.cloudId;
                        item.creditLimit = data.creditLimit;
                        item.email = data.email;
                        item.leadTime = data.leadTime;
                        item.localId = data.localId;
                        item.taxId = data.taxId;
                        item.telephone = data.telephone;
                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.Contact contact = new Contact();
                    contact.company = company;
                    contact.updatedAt = Convert.ToDateTime(data.updatedAt);
                    contact.updatedAt = contact.updatedAt.Value.ToLocalTime();
                    contact.createdAt = Convert.ToDateTime(data.createdAt);
                    contact.createdAt = contact.createdAt.Value.ToLocalTime();

                    contact.address = data.address;
                    contact.alias = data.alias;
                    contact.cloudId = data.cloudId;
                    contact.creditLimit = data.creditLimit;
                    contact.email = data.email;
                    contact.leadTime = data.leadTime;
                    contact.localId = data.localId;
                    contact.taxId = data.taxId;
                    contact.telephone = data.telephone;

                    ctx.Contacts.Add(contact);
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {

                    int localId = (int)data.localId;
                    Models.Contact item = ctx.Contacts.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        item.updatedAt = Convert.ToDateTime(data.updatedAt);
                        item.updatedAt = item.updatedAt.Value.ToLocalTime();
                        item.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {

                        item.cloudId = data.cloudId;
                        item.updatedAt = Convert.ToDateTime(data.updatedAt);
                        item.updatedAt = item.updatedAt.Value.ToLocalTime();
                        item.createdAt = Convert.ToDateTime(data.createdAt);
                        item.createdAt = item.createdAt.Value.ToLocalTime();
                    }
                   
                 
                }
            }
            ctx.SaveChanges();
        }
        public dynamic Updatedata(Cognitivo.API.Models.Customer Customer,Core.Models.Contact item)
        {
            Customer.updatedAt = item.updatedAt != null ? item.updatedAt.Value.ToUniversalTime() : item.createdAt.Value.ToUniversalTime();
            Customer.action = (Cognitivo.API.Enums.Action)item.action;
            Customer.address = item.address;
            Customer.alias = item.alias;
            Customer.cloudId = item.cloudId;
            Customer.createdAt = item.createdAt != null ? item.updatedAt.Value.ToUniversalTime() : item.createdAt.Value.ToUniversalTime();
            Customer.creditLimit = item.creditLimit;
            Customer.deletedAt = item.deletedAt != null ? item.deletedAt.Value : item.deletedAt;
            Customer.email = item.email;
            Customer.leadTime = item.leadTime;
            Customer.localId = item.localId;
            Customer.taxId = item.taxId;
            Customer.telephone = item.telephone;
            return Customer;

        }
    }
}
