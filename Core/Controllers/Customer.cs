using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Core.Controllers
{
    public class Customer
    {
        /// <summary>
        /// Gets or sets the customers.
        /// </summary>
        /// <value>The customers.</value>
        IQueryable<Models.Contact> Customers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.ViewModels.Commercial.Customer"/> class.
        /// </summary>
        public Customer(int Take = 25, int Skip = 0)
        {
            using (Models.Context ctx = new Models.Context())
            {

                //IQueryable<Models.Contact> customers = ctx.GetQueryableProducts();
                Customers = ctx.Contacts.Where(p => p.IsCustomer);

                //outside
                Customers.Where(x => x.IsSupplier).Take(Take).AsQueryable();

                ctx.Contacts.Where(x => x.IsCustomer)
                                     .Take(Take)
                   .Skip(Skip);
            }
        }

        /// <summary>
        /// Search the specified Query.
        /// </summary>
        /// <param name="Query">Query.</param>
        public void Search(string Query)
        {
            using (Models.Context ctx = new Models.Context())
            {
                Customers = ctx.Contacts
                               .Where(x => x.IsCustomer &&  (
                                          x.Name.Contains(Query) || 
                                          x.TaxID.Contains(Query))
                                     );
            }
        }

        /// <summary>
        /// Filters the Contacts list against local data.
        /// </summary>
        /// <param name="Query">Query.</param>
        public void Filter(string Query)
        {
            using (Models.Context ctx = new Models.Context())
            {
                //Customers = ctx.Contacts.Local
                               //.Where(x => 
                               //       x.Name.Contains(Query) || 
                               //       x.TaxID.Contains(Query) || 
                               //       x.Telephone.Contains(Query) ||
                               //       x.Email.Contains(Query) ||
                               //      x.Address.Contains(Query))
                               //.ToList();
            }
        }

        /// <summary>
        /// Gets the last five sales made by this customer.
        /// </summary>
        /// <returns>The last five sales.</returns>
        /// <param name="Contact">Customer (aka Contact).</param>
        public void GetLastFiveSales(Models.Contact Contact, int Take = 25, int Skip = 0)
        {
            using (Models.Context ctx = new Models.Context())
            {
                ctx.Orders.Where(x => x.Contact.Id == Contact.Id)
                               .Include(y => y.Details)
                               .Include(z => z.Contact)
                               .Take(Take)
                               .Skip(Skip)
                               .Load();
            }
        }

        /// <summary>
        /// Save the specified Contact.
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="Contact">Contact.</param>
        public Models.Contact Save(Models.Contact Contact)
        {
            using (Models.Context ctx = new Models.Context())
            {
                ctx.Attach(Contact);
                ctx.SaveChanges();
                return Contact;
            }
        }

        /// <summary>
        /// Delete the specified Contact.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="Contact">Contact.</param>
        public bool Delete(Models.Contact Contact)
        {
            using (Models.Context ctx = new Models.Context())
            {
                ctx.Attach(Contact);
                ctx.Contacts.Remove(Contact);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}
