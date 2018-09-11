using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Order
    {
        public enum Types { Sales, Purchase }

        public Order()
        {
            Date = DateTime.Now;
            PaymentCondition = 0;
            Details = new List<OrderDetail>();
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public Types OrderType { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company Company { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the point of sale.
        /// </summary>
        /// <value>The point of sale.</value>
        public PointOfSale PointOfSale { get; set; }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>The range.</value>
        public Range Range { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        public Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the invoice code.
        /// </summary>
        /// <value>The invoice code.</value>
        public string InvoiceCode { get; set; }

        /// <summary>
        /// Gets or sets the payment condition.
        /// </summary>
        /// <value>The payment condition.</value>
        public int PaymentCondition { get; set; }

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
    }
}
