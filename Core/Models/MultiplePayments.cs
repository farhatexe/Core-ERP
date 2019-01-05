using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class MultiplePayments
    {
        public Core.Models.PaymentType PaymentType { get; set; }
        public Decimal Amount { get; set; }
    }
}
