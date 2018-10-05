using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Models
{
    public class Order
    {
        /// <summary>
        /// Types.
        /// </summary>
        public enum Types { Sales, Purchase }

        public Order()
        {
            Date = DateTime.Now;
            Details = new List<OrderDetail>();
            IsArchived = false;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public Types OrderType { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public Enums.Status Status { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company Company { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location Location { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>Session used during current order.</value>
        public Session Session { get; set; }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>Range used during current order.</value>
        public Range Range { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        public Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the payment contract.
        /// </summary>
        /// <value>The payment contract.</value>
        public PaymentContract PaymentContract { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the invoice code.
        /// </summary>
        /// <value>The invoice code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the code expiry.
        /// </summary>
        /// <value>The code expiry.</value>
        public DateTime CodeExpiry { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the currency rate.
        /// </summary>
        /// <value>The currency rate.</value>
        public decimal CurrencyRate { get; set; }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <value>The total.</value>
        public decimal Total { get { return Details.Sum(x => x.SubTotalVat); } }

        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public TimeSpan Interval { get{ return Date - DateTime.Now; } }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>The details.</value>
        public List<OrderDetail> Details { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.Order"/> is archived.
        /// </summary>
        /// <value><c>true</c> if is archived; otherwise, <c>false</c>.</value>
        public bool IsArchived { get; set; }
    }
}
