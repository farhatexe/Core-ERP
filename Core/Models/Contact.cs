using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    /// <summary>
    /// Contact.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company Company { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.Orders.Contact"/> is customer.
        /// </summary>
        /// <value><c>true</c> if is customer; otherwise, <c>false</c>.</value>
        public bool IsCustomer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.Orders.Contact"/> is supplier.
        /// </summary>
        /// <value><c>true</c> if is supplier; otherwise, <c>false</c>.</value>
        public bool IsSupplier { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tax identifier.
        /// </summary>
        /// <value>The tax identifier.</value>
        public string TaxID { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the telephone.
        /// </summary>
        /// <value>The telephone.</value>
        public string Telephone { get; set; }

        /// <summary>
        /// Gets or sets the lead time.
        /// </summary>
        /// <value>The lead time.</value>
        public int? LeadTime { get; set; }

        /// <summary>
        /// Gets or sets the credit limit.
        /// </summary>
        /// <value>The credit limit.</value>
        public int? CreditLimit { get; set; }
    }
}
