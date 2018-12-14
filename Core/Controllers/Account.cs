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

        public void ReceivePayments(List<Models.Order> orders, Models.Account account, Models.PaymentType paymentType, DateTime paymentDate, string currencyCode, decimal currencyRate, decimal amount)
        {
            decimal balance = amount;

            foreach (var order in orders.Where(x => x.type == Models.Order.Types.Sales))
            {
                if (balance >= order.total)
                {
                    foreach (var contract in order.paymentContract.details.Where(x => x.forOrders == false))
                    {
                        AccountMovement movement = new AccountMovement()
                        {
                            account = account,
                            order = order,
                            paymentType = paymentType,
                            date = paymentDate,
                            debit = 0,
                            credit = order.total * contract.percentage,
                            currencyCode = currencyCode,
                            currencyRate = currencyRate,
                        };

                        _db.AccountMovements.Add(movement);
                    }

                    balance -= order.total;
                }
                else
                {
                    foreach (var contract in order.paymentContract.details.Where(x => x.forOrders == false))
                    {
                        if (balance >= (order.total * contract.percentage))
                        {
                            AccountMovement movement = new AccountMovement()
                            {
                                account = account,
                                order = order,
                                paymentType = paymentType,
                                date = paymentDate,
                                debit = 0,
                                credit = order.total * contract.percentage,
                                currencyCode = currencyCode,
                                currencyRate = currencyRate,
                            };
                            _db.AccountMovements.Add(movement);
                        }
                        else
                        {
                            AccountMovement movement = new AccountMovement()
                            {
                                account = account,
                                order = order,
                                paymentType = paymentType,
                                date = paymentDate,
                                debit = 0,
                                credit = balance,
                                currencyCode = currencyCode,
                                currencyRate = currencyRate,
                            };
                            _db.AccountMovements.Add(movement);

                            Models.PaymentSchedual schedual = new Models.PaymentSchedual()
                            {
                                amountOwed = (order.total * contract.percentage) - balance,
                                order = order,
                                date = order.date.AddDays(contract.offset),
                                comment = "Amount not qualified."
                            };

                            balance -= movement.credit;
                        }
                    }
                }
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
