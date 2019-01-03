using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.ObjectModel;

namespace Core.Models
{
    public class Order : BaseClass
    {
        /// <summary>
        /// Types.
        /// </summary>
        public enum Types { Sales, Purchase }

        public Order()
        {
            date = DateTime.Now;
            details = new ObservableCollection<OrderDetail>();
            isArchived = false;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int? cloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public Types type { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public Enums.Status status { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public virtual Company company { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public virtual Location location { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public virtual DateTime date { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>Session used during current order.</value>
        public virtual Session session { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>Range used during current order.</value>
        public virtual Range range { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public virtual Contact customer { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the payment contract.
        /// </summary>
        /// <value>The payment contract.</value>
        public virtual PaymentContract paymentContract { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string invoiceNumber { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the invoice code.
        /// </summary>
        /// <value>The invoice code.</value>
        public string code { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the code expiry.
        /// </summary>
        /// <value>The code expiry.</value>
        public DateTime codeExpiry { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string currency { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the currency rate.
        /// </summary>
        /// <value>The currency rate.</value>
        public decimal currencyRate { get; set; }

        [NotMapped]
        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <value>The total.</value>
        public decimal total { get { return details.Sum(x => x.subTotalVat); } }

        [NotMapped]
        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public TimeSpan interval { get{ return date - DateTime.Now; } }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>The details.</value>
        public virtual ObservableCollection<OrderDetail> details { get; set; }

        /// <summary>
        /// Get or Sets Movements
        /// </summary>
        public virtual ObservableCollection<AccountMovement> movements { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.Order"/> is archived.
        /// </summary>
        /// <value><c>true</c> if is archived; otherwise, <c>false</c>.</value>
        public bool isArchived { get; set; }

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

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [NotMapped]
        public Message.Warning? message { get; set; }
    }
}
