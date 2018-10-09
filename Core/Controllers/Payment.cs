using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
   public class PaymentController
    {
        private Context ctx;

        public PaymentController(Context DB)
        {
            ctx = DB;
        }

   

        public void ReceivePayment(Order Order,PaymentType paymenttype)
        {
            List<PaymentSchedual> PaymentSchedualList = ctx.PaymentSchedual.Where(x => x.Order == Order).ToList();
            foreach (PaymentSchedual schedual in PaymentSchedualList)
            {
                Payment Payment = new Payment();
                Payment.AmountOwed = schedual.AmountOwed;
                Payment.Order = Order;
                Payment.Date = Order.Date;
                Payment.PaymentType = paymenttype;
                Payment.PaymentSchedual = schedual;
                ctx.payments.Add(Payment);
            }
         
        }
    }
}
