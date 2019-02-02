using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class Session
    {
        private Context _db;
        public Session(Context db)
        {
            _db = db;
        }

        public Core.Models.Session OpenSession(PointOfSale pointOfSale, string currencyCode, decimal currencyRate = 1, decimal addToBalance = 0)
        {
            //foreach currency starting balance.
            decimal accountBalance = pointOfSale.defaultAccount.accountMovements.Where(x => x.currencyCode == currencyCode).Sum(x => (x.credit - x.debit) * x.currencyRate);
            Core.Models.Session session = new Core.Models.Session();
            session.name = "Sales started on: " + DateTime.Now.Date;
            session.startDate = DateTime.Now;
            session.startingBalance = accountBalance + addToBalance;
            session.PointOfSale = pointOfSale;

            if (accountBalance!=addToBalance)
            {
                decimal Balance = accountBalance - addToBalance;
                if (Balance > 0)
                {
                    AccountMovement movement = new AccountMovement()
                    {
                        account = pointOfSale.defaultAccount,
                        paymentType = pointOfSale.defaultPaymentType,
                        date = session.startDate,
                        debit = Balance,
                        credit = 0,
                        currencyCode = currencyCode,
                        currencyRate = currencyRate,
                        comment = "Added for Opening Balance"
                    };

                    session.movements.Add(movement);
                }
                else
                {
                    AccountMovement movement = new AccountMovement()
                    {
                        account = pointOfSale.defaultAccount,
                        paymentType = pointOfSale.defaultPaymentType,
                        date = session.startDate,
                        debit = 0,
                        credit = Balance,
                        currencyCode = currencyCode,
                        currencyRate = currencyRate,
                        comment = "Added for Opening Balance"
                    };

                    session.movements.Add(movement);
                }
            }
            //todo need to fix currency rate
           

            _db.Sessions.Add(session);
            
            return session;
        }

        public Core.Models.Session CloseSession(Core.Models.Session session, string currencyCode, decimal currencyRate = 1, decimal keepingValue = 0, Core.Models.Account transferAccount = null)
        {
            session.endDate = DateTime.Now;
            session.endingBalance = session.CurrentEndingBalance;

            decimal valueTransfered = session.CurrentEndingBalance - keepingValue;

            if (valueTransfered  > 0)
            {
                AccountMovement debitMovement = new AccountMovement()
                {
                    comment = "Transfer or Difference",
                    account = session.PointOfSale.defaultAccount,
                    paymentType = session.PointOfSale.defaultPaymentType,
                    date = Convert.ToDateTime(session.endDate),
                    debit = session.CurrentEndingBalance - keepingValue,
                    credit = 0,
                    currencyCode = session.PointOfSale.defaultAccount.currencyCode,
                    currencyRate = 1,
                };
                session.movements.Add(debitMovement);

                if (transferAccount != null)
                {
                    AccountMovement creditMovement = new AccountMovement()
                    {
                        comment = "Transfer from: " + session.PointOfSale.defaultAccount.name,
                        account = transferAccount,
                        paymentType = session.PointOfSale.defaultPaymentType,
                        date = Convert.ToDateTime(session.endDate),
                        credit = valueTransfered,
                        debit = 0,
                        currencyCode = session.PointOfSale.defaultAccount.currencyCode,
                        currencyRate = 1,
                    };
                    session.movements.Add(creditMovement);
                }
            }

            _db.SaveChanges();
            return session;

        }
    }
}
