using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
   public class PaymentSchedualController
    {
        private Context ctx;

        public PaymentSchedualController(Context DB)
        {
            ctx = DB;
        }

      
        public void ReceivePayment(Order Order)
        {
            PaymentSchedual paymentschedual = new PaymentSchedual();
            paymentschedual.AmountOwed = Order.Details.Sum(x => x.SubTotalVat);
            paymentschedual.Order = Order;
            paymentschedual.Date = Order.Date;
            ctx.PaymentSchedual.Add(paymentschedual);
        }
    }
}
