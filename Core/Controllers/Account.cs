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
        /// <param name="paymentType">List of Multiple Payment type like cash card.</param>
        /// <param name="paymentDate">Payment date.</param>
        /// <param name="currencyCode">Currency code.</param>
        /// <param name="currencyRate">Currency rate.</param>
        /// <param name="currentObligation">Current obligation.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="contractOffset">Contract offset.</param>
        public decimal ReceivePayment(List<Models.Order> order,Models.Account account, ObservableCollection<MultiplePayments> MultiplePaymnets, DateTime paymentDate, string currencyCode, decimal currencyRate, decimal currentObligation, decimal balance = 0, int contractOffset = 0)
        {
            foreach (MultiplePayments item in MultiplePaymnets)
            {
             ReceivePayments(order, account, item.PaymentType, DateTime.Now, currencyCode, 1, item.Amount);
            }
            return 0;
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
                Cognitivo.API.Models.Account Account = new Cognitivo.API.Models.Account();
                Account = Updatedata(Account, item);
                syncList.Add(Account);
            }
            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Account);
            foreach (dynamic data in ReturnItem)
            {

                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.Account account = _db.Accounts.Where(x => x.localId == localId).FirstOrDefault();
                    if (data.deletedAt != null)
                    {
                        account.updatedAt = Convert.ToDateTime(data.updatedAt);
                        account.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        account.updatedAt = Convert.ToDateTime(data.updatedAt);
                        account.updatedAt = account.updatedAt.Value.ToLocalTime();
                        account.createdAt = Convert.ToDateTime(data.createdAt);
                        account.createdAt = account.createdAt.Value.ToLocalTime();
                        account.name = data.name;
                        account.number = data.number;
                        account.cloudId = data.cloudId;
                        account.currencyCode = data.currencyCode;

                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.Account account = new Models.Account();
                    account.updatedAt = Convert.ToDateTime(data.updatedAt);
                    account.updatedAt = account.updatedAt.Value.ToLocalTime();
                    account.createdAt = Convert.ToDateTime(data.createdAt);
                    account.createdAt = account.createdAt.Value.ToLocalTime();
                    account.name = data.name;
                    account.number = data.number;
                    account.cloudId = data.cloudId;
                    account.currencyCode = data.currencyCode;

                    _db.Accounts.Add(account);
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.Account account = _db.Accounts.Where(x => x.localId == localId).FirstOrDefault();
                    account.updatedAt = Convert.ToDateTime(data.updatedAt);
                    account.updatedAt = account.updatedAt.Value.ToLocalTime();
                    account.createdAt = Convert.ToDateTime(data.createdAt);
                    account.createdAt = account.createdAt.Value.ToLocalTime();
                }
            }
            _db.SaveChanges();
        }
        public dynamic Updatedata(Cognitivo.API.Models.Account Account, Core.Models.Account item)
        {
            Account.updatedAt = item.updatedAt != null ? item.updatedAt.Value : item.createdAt.Value; ;
            Account.action = (Cognitivo.API.Enums.Action)item.action;
            Account.name = item.name;
            Account.number = item.number;
            Account.cloudId = item.cloudId;
            Account.createdAt = item.createdAt != null ? item.updatedAt.Value : item.createdAt.Value; ;
            Account.currencyCode = item.currencyCode;
            Account.deletedAt = item.deletedAt != null ? item.deletedAt.Value : item.deletedAt;
            Account.localId = item.localId;
            return Account;

        }

    }
}
