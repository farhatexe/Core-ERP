using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Payment type.
    /// </summary>
    public class PaymentType : BaseClass
    {
        /// <summary>
        /// Core accepts different types of behaviours.`
        /// </summary>
        public enum Behaviors
        { 
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
        [DataMember]
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        [DataMember]
        public Company company { get; set; }

        /// <summary>
        /// Gets or sets the behavior.
        /// </summary>
        /// <value>The behavior.</value>
        [DataMember]
        public Behaviors behavior { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        [DataMember]
        public string country { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        [DataMember]
        public string icon { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime? createdAt { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime? updatedAt { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the deleted at.
        /// </summary>
        /// <value>The deleted at.</value>
        public DateTime? deletedAt { get; set; }

        [NotMapped]
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public int action { get; set; }

        [DataMember]
        public List<PointOfSale> pointOfSales { get; set; }

        public List<AccountMovement> movements { get; set; }
    }
}
