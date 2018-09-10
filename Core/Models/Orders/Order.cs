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
            date = DateTime.Now;
            paymentCondition = 0;
            details = new List<OrderDetail>();
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

        public decimal Total { get { return Details.Sum(x => (x.quantity * x.price)); } }

        public TimeSpan interval { get{ return Date - DateTime.Now; } }
        public List<OrderDetail> Details { get; set; }
    }
}
