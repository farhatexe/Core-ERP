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
            Customers = ctx.Contacts
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
            ctx.Contacts.Remove(Contact);
            ctx.SaveChanges();
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

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> CustomerList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.Customer);
            foreach (dynamic data in CustomerList)
            {
                Contact contact = new Core.Models.Contact
                {
                    cloudId = data.cloudId,
                    alias = data.name,
                    taxId = data.taxid,
                    address = data.address,
                    email = data.email,
                    telephone = data.telephone,
                    leadTime = data.leadTime,
                    creditLimit = data.creditLimit != null ? (int)data.creditLimit : 0
                };
                ctx.Contacts.Add(contact);

            }
            ctx.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.Contact item in ctx.Contacts.ToList())
            {
                item.createdAt = item.createdAt;
                item.updatedAt = item.createdAt;
                syncList.Add(item);
            }
            CognitivoAPI.UploadData(slug, "", syncList, API.CognitivoAPI.Modules.Customer);

        }
    }
}
