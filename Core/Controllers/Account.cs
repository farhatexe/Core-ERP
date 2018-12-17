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
        /// Receives the payment.
        /// </summary>
        /// <returns>The Balance after Payment</returns>
        /// <param name="order">Order.</param>
        /// <param name="account">Account.</param>
        /// <param name="paymentType">Payment type.</param>
        /// <param name="paymentDate">Payment date.</param>
        /// <param name="currencyCode">Currency code.</param>
        /// <param name="currencyRate">Currency rate.</param>
        /// <param name="currentObligation">Current obligation.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="contractOffset">Contract offset.</param>
        public decimal ReceivePayment(Models.Order order, Models.PaymentSchedual schedual, Models.Account account, Models.PaymentType paymentType, DateTime paymentDate, string currencyCode, decimal currencyRate, decimal currentObligation, decimal balance = 0, int contractOffset = 0)
        {
            AccountMovement movement = new AccountMovement()
            {
                account = account,
                order = order,
                schedual = schedual, // Nullable
                paymentType = paymentType,
                date = paymentDate,
                debit = 0,
                credit = balance >= currentObligation ? currentObligation : balance,
                currencyCode = currencyCode,
                currencyRate = currencyRate,
            };

            _db.AccountMovements.Add(movement);
            balance -= currentObligation;

            //In case the current obligation is greater than the balance of the current payment, then make a schedual to have it paid at a later date based on the current obligation's contract.
            if (balance < currentObligation && schedual == null)
            {
                Models.PaymentSchedual newSchedual = new Models.PaymentSchedual()
                {
                    amountOwed = currentObligation - balance,
                    order = order,
                    date = order.date.AddDays(contractOffset),
                    comment = "Amount not qualified."
                };
                _db.PaymentSchedual.Add(newSchedual);
            }

            return balance;
        }

        /// <summary>
        /// Code to Recieve Payments based on Order instead of Payment Schedual
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

                if (order.paymentContract != null)
                {
                    foreach (var contract in order.paymentContract.details.Where(x => x.forOrders == false))
                    {
                        decimal currentObligation = (order.total * currencyRate) * contract.percentage;
                        balance = ReceivePayment(order, null, account, paymentType, paymentDate, currencyCode, currencyRate, currentObligation, balance, contract.offset);
                    }
                }
                else
                {
                    decimal currentObligation = order.total * currencyRate;
                    balance = ReceivePayment(order, null, account, paymentType, paymentDate, currencyCode, currencyRate, currentObligation, balance);
                }
            }
        }

        /// <summary>
        /// Code to Recieve Payments based on Payment Schedual
        /// </summary>
        /// <param name="scheduals">Scheduals.</param>
        /// <param name="account">Account.</param>
        /// <param name="paymentType">Payment type.</param>
        /// <param name="paymentDate">Payment date.</param>
        /// <param name="currencyCode">Currency code.</param>
        /// <param name="currencyRate">Currency rate.</param>
        /// <param name="amount">Amount.</param>
        public void ReceivePayments(List<Models.PaymentSchedual> scheduals, Models.Account account, Models.PaymentType paymentType, DateTime paymentDate, string currencyCode, decimal currencyRate, decimal amount)
        {
            decimal balance = amount * currencyRate;

            foreach (var schedual in scheduals)
            {
                decimal currentObligation = schedual.amountOwed * currencyRate;
                balance = ReceivePayment(schedual.order, schedual, account, paymentType, paymentDate, currencyCode, currencyRate, currentObligation, balance, 0);
            }
        }

        public void Download(string slug, string key)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> AccountList = CognitivoAPI.DowloadData(slug, key, Core.API.CognitivoAPI.Modules.Account);

            foreach (dynamic
                data in AccountList)
            {
                int cloudId = (int)data.cloudId;
                Models.Account account = _db.Accounts.Where(x => x.cloudId == cloudId).FirstOrDefault() ?? new Models.Account();

                account.cloudId = data.cloudId;
                account.name = data.name;
                account.number = data.number;
                account.currencyCode = data.currencyCode;
                if (account.localId == 0)
                {
                    _db.Accounts.Add(account);
                }

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
