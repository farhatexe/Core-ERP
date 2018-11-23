using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
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
        IQueryable<Models.Customer> Customers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.ViewModels.Commercial.Customer"/> class.
        /// </summary>
        public ObservableCollection<Customer> LIST(int Take = 25, int Skip = 0)
        {
            Customers = ctx.Contacts
                               .Take(Take).Skip(Skip);

            return new ObservableCollection<Customer>(Customers);

        }

        /// <summary>
        /// Search the specified Query.
        /// </summary>
        /// <param name="Query">Query.</param>
        public ObservableCollection<Customer> Search(string Query)
        {

            Customers = ctx.Contacts
                           .Where(x =>(
                                      x.alias.Contains(Query) ||
                                      x.taxId.Contains(Query))
                                 );
            return new ObservableCollection<Customer>(Customers);

        }

        /// <summary>
        /// Filters the Contacts list against local data.
        /// </summary>
        /// <param name="Query">Query.</param>
        public ObservableCollection<Customer> Filter(string Query)
        {

            Customers = ctx.Contacts
            .Where(x =>
                   x.alias.Contains(Query) ||
                   x.taxId.Contains(Query) ||
                   x.telephone.Contains(Query) ||
                   x.email.Contains(Query) ||
                  x.address.Contains(Query));

            return new ObservableCollection<Customer>(Customers);


        }

        /// <summary>
        /// Gets the last five sales made by this customer.
        /// </summary>
        /// <returns>The last five sales.</returns>
        /// <param name="Contact">Customer (aka Contact).</param>
        public List<Models.Order> GetLastFiveSales(Models.Customer Contact, int Take = 25, int Skip = 0)
        {

            return ctx.Orders.Where(x => x.contact.localId == Contact.localId)
                                 .Include(y => y.details)
                                 .Include(z => z.contact)
                                 .Take(Take)
                                 .Skip(Skip).ToList();




        }

        /// <summary>
        /// Save the specified Contact.
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="Contact">Contact.</param>
        public Models.Customer Save(Models.Customer Contact)
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
        public bool Delete(Models.Customer Contact)
        {

            ctx.Contacts.Remove(Contact);
            ctx.SaveChanges();
            return true;

        }
        /// <summary>
        /// Add the specified Entity.
        /// </summary>
        /// <param name="Entity">Entity.</param>
        public void Add(Models.Customer Entity)
        {
            ctx.Contacts.Add(Entity);
        }

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> CustomerList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.Customer);
            foreach (dynamic data in CustomerList)
            {
                Customer contact = new Core.Models.Customer
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
            List<object> CustomerList = ctx.Contacts.Cast<object>().ToList();
            CognitivoAPI.UploadData(slug, "", CustomerList, Core.API.CognitivoAPI.Modules.Customer);

        }
    }
}
