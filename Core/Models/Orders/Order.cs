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

        public int id { get; set; }
        public int cloudID { get; set; }

        public Types orderType { get; set; }

        public DateTime date { get; set; }
        public Contact contact { get; set; }
        public string invoiceNumber { get; set; }
        public string invoiceCode { get; set; }
        public int paymentCondition { get; set; }

        public string currency { get; set; }
        public decimal currencyRate { get; set; }

        public decimal total { get { return details.Sum(x => (x.quantity * x.price)); } }

        public TimeSpan interval { get{ return date - DateTime.Now; } }
        public List<OrderDetail> details { get; set; }
    }
}
