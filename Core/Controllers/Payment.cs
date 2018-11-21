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

   

        public void ReceivePayment(Models.Order Order,PaymentType paymenttype)
        {
            List<Models.PaymentSchedual> PaymentSchedualList = ctx.PaymentSchedual.Where(x => x.order == Order).ToList();
            foreach (Models.PaymentSchedual schedual in PaymentSchedualList)
            {
                Payment Payment = new Payment();
                Payment.amountOwed = schedual.amountOwed;
                Payment.order = Order;
                Payment.date = Order.date;
                Payment.paymentType = paymenttype;
                Payment.schedualId = schedual.localId;
                ctx.payments.Add(Payment);
            }
         
        }
    }
}
