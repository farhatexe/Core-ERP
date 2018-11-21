using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    /// <summary>
    /// Payment type.
    /// </summary>
    public class PaymentType
    {
        /// <summary>
        /// Core accepts different types of behaviours.
        /// </summary>
        public enum Behaviours { 
            /// <summary>
            /// Normal: Such as Cash, Check, Credit Card.
            /// </summary>
            Normal, 
            /// <summary>
            /// Same as Normal, except the money enters the account delayed. This requires an aditional process of varification before acrediting value into account.
            /// </summary>
            Delayed,
            /// <summary>
            /// Credit Behaviours are meant for Credit and Debit Notes. Where you use credits to pay off debt.
            /// </summary>
            Credit, 
            /// <summary>
            /// Vat WithHolding depending on certain countries that require Customers or Suppliers to Withhold vat generated from the transaction.
            /// </summary>
            VatWithholding 
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company company { get; set; }

        /// <summary>
        /// Gets or sets the behaviour.
        /// </summary>
        /// <value>The behaviour.</value>
        public Behaviours behaviour { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public string icon { get; set; }

        public List<Payment> payments { get; set; }
        public List<PointOfSale> pointOfSales { get; set; }
    }
}
