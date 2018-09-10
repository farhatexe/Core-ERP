using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models.Orders
{
    public class Order
    {
        public enum Types { Sales, Purchases }

        public Order()
        {
            Date = DateTime.Now;
            PaymentCondition = 0;
            Details = new List<OrderDetail>();
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public Types OrderType { get; set; }

        public DateTime Date { get; set; }
        public Contact Contact { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCode { get; set; }
        public int PaymentCondition { get; set; }

        public string Currency { get; set; }
        public decimal CurrencyRate { get; set; }

        public decimal Total { get { return Details.Sum(x => (x.Quantity * x.Price)); } }

        public TimeSpan interval { get{ return Date - DateTime.Now; } }
        public List<OrderDetail> Details { get; set; }
    }
}
