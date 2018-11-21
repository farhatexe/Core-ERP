using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
   public class PaymentSchedual
    {
        private Context ctx;

        public PaymentSchedual(Context DB)
        {
            ctx = DB;
        }

      
        public void ReceivePayment(Models.Order Order)
        {
            Models.PaymentSchedual paymentschedual = new Models.PaymentSchedual();
            paymentschedual.amountOwed = Order.details.Sum(x => x.subTotalVat);
            paymentschedual.order = Order;
            paymentschedual.date = Order.date;
            ctx.PaymentSchedual.Add(paymentschedual);
        }
    }
}
