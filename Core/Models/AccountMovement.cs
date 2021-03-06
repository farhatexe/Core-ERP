﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
   public enum Types
    {
        Starting,
        Transaction,
        Closing,
    }
    /// <summary>
    /// All money account movements such as inflows and outflows are registered in this table.
    /// </summary>
    public class AccountMovement : BaseClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int? cloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the account cloud identifier.
        /// </summary>
        /// <value>The  account cloud identifie.</value>
        public int accountCloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        public virtual Session session { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        public virtual Account account { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>The type of the payment.</value>
        [DataMember]
        public virtual PaymentType paymentType { get; set; }

        /// <summary>
        /// Gets or sets the schedual.
        /// </summary>
        /// <value>The schedual.</value>
        [DataMember]
        public virtual PaymentSchedual schedual { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        [DataMember]
        public virtual Order order { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime date { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the debit.
        /// </summary>
        /// <value>The debit.</value>
        public decimal debit { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>The credit.</value>
        public decimal credit { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string currencyCode { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the rate against the default currency
        /// </summary>
        /// <value>The rate.</value>
        public decimal currencyRate { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string comment { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        /// <value>The debit.</value>
        public Types type { get; set; }

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
         //<summary>
         //Gets or sets the deleted at.
         //</summary>
         //<value>The deleted at.</value>
        public DateTime? deletedAt { get; set; }

        [NotMapped]
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public int action { get; set; }
    }
}
