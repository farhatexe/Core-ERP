using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class Account
    {
        private Context _db;

        public Account(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.Account> List()
        {
            _db.Accounts.Load();
            return _db.Accounts.Local.ToObservableCollection();
        }

        public void Add(Models.Account Entity)
        {
            _db.Accounts.Add(Entity);
        }

        public void Delete(Models.Account Entity)
        {
            _db.Accounts.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Function will take your order (sales) and receive the payment.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <param name="account">Account.</param>
        /// <param name="paymentType">Payment type.</param>
        /// <param name="paymentDate">Payment date.</param>
        /// <param name="currencyCode">Currency code.</param>
        /// <param name="currencyRate">Currency rate.</param>
        /// <param name="amount">Amount.</param>
        public void ReceivePayment(Models.Order order, Models.Account account, Models.PaymentType paymentType, DateTime paymentDate, string currencyCode, decimal currencyRate, decimal amount, decimal balance = 0)
        {
            //fixes an issue if developer does not assign a balance.
            balance = balance > 0 ? balance : amount;

            foreach (var contract in order.paymentContract.details.Where(x => x.forOrders == false))
            {
                decimal currentObligation = (order.total * currencyRate) * contract.percentage;

                AccountMovement movement = new AccountMovement()
                {
                    account = account,
                    order = order,
                    paymentType = paymentType,
                    date = paymentDate,
                    debit = 0,
                    credit = balance >= currentObligation ? currentObligation : balance,
                    currencyCode = currencyCode,
                    currencyRate = currencyRate,
                };

                _db.AccountMovements.Add(movement);

                //In case the current obligation is greater than the balance of the current payment, then make a schedual to have it paid at a later date based on the current obligation's contract.
                if (balance < currentObligation)
                {
                    Models.PaymentSchedual schedual = new Models.PaymentSchedual()
                    {
                        amountOwed = currentObligation - balance,
                        order = order,
                        date = order.date.AddDays(contract.offset),
                        comment = "Amount not qualified."
                    };

                    balance -= movement.credit;
                }
            }
        }

        /// <summary>
        /// Will take your <list type="order"/> and run the Receive Payment function.
        /// </summary>
        /// <param name="orders">Orders.</param>
        /// <param name="account">Account.</param>
        /// <param name="paymentType">Payment type.</param>
        /// <param name="paymentDate">Payment date.</param>
        /// <param name="currencyCode">Currency code.</param>
        /// <param name="currencyRate">Currency rate.</param>
        /// <param name="amount">Amount.</param>
        public void ReceivePayments(List<Models.Order> orders, Models.Account account, Models.PaymentType paymentType, DateTime paymentDate, string currencyCode, decimal currencyRate, decimal amount)
        {
            decimal balance = amount * currencyRate;

            foreach (var order in orders.Where(x => x.type == Models.Order.Types.Sales))
            {
                ReceivePayment(order, account, paymentType, paymentDate, currencyCode, currencyRate, order.total, balance);
            }
        }

        public void ReceivePayments(List<Models.PaymentSchedual> scheduals, Models.Account account, Models.PaymentType paymentType, DateTime paymentDate, string currencyCode, decimal currencyRate, decimal amount)
        {
            decimal balance = amount * currencyRate;

            foreach (var schedual in scheduals)
            {
                decimal currentObligation = schedual.amountOwed * currencyRate;
                AccountMovement movement = new AccountMovement()
                {
                    account = account,
                    schedual = schedual,
                    order = schedual.order,
                    paymentType = paymentType,
                    date = paymentDate,
                    debit = 0,
                    credit = balance >= currentObligation ? currentObligation : balance,
                    currencyCode = currencyCode,
                    currencyRate = currencyRate,
                };

                _db.AccountMovements.Add(movement);

                balance -= currentObligation;
            }
        }

        public void Download(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> AccountList = CognitivoAPI.DowloadData(slug, "", Core.API.CognitivoAPI.Modules.Account);
            
            foreach (dynamic data in AccountList)
            {
                Models.Account account = new Core.Models.Account
                {
                    cloudId = data.cloudId,
                    name = data.name,
                    number = data.number,
                    currencyCode = data.currencyCode,

                };
                _db.Accounts.Add(account);

            }
          _db.SaveChanges();
        }

        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();
            foreach (Core.Models.Account item in _db.Accounts.ToList())
            {
                item.createdAt = item.createdAt;
                item.updatedAt = item.createdAt;
                syncList.Add(item);
            }
            CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Account);

        }

    }
}
