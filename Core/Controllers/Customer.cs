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
    public class Customer
    {

        private Context ctx;

        public Customer(Context db)
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
            Customers = ctx.Contacts.Where(x => x.IsCustomer)
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
                           .Where(x => x.IsCustomer && (
                                      x.Name.Contains(Query) ||
                                      x.TaxID.Contains(Query))
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
                   x.Name.Contains(Query) ||
                   x.TaxID.Contains(Query) ||
                   x.Telephone.Contains(Query) ||
                   x.Email.Contains(Query) ||
                  x.Address.Contains(Query));

            return new ObservableCollection<Contact>(Customers);


        }

        /// <summary>
        /// Gets the last five sales made by this customer.
        /// </summary>
        /// <returns>The last five sales.</returns>
        /// <param name="Contact">Customer (aka Contact).</param>
        public List<Models.Order> GetLastFiveSales(Models.Contact Contact, int Take = 25, int Skip = 0)
        {

            return ctx.Orders.Where(x => x.Contact.Id == Contact.Id)
                                 .Include(y => y.Details)
                                 .Include(z => z.Contact)
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
    }
}
