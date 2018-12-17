using Core.Models;
using Core.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class UnitOfWork
    {
        private ItemController itemRepository;
        private CustomerController customerRepo;
        private DocumentController rangeRepo;
        private InventoryController inventoryRepo;
        private VatController vatRepo;
        private Controllers.Account accountRepo;
        private ItemCategoryController itemcategoryRepo;
        private Sales salesRepo;
        private Controllers.PaymentType paymentTypeRepo;
        private Controllers.PaymentSchedual paymentSchedualRepo;
        private Controllers.Session sessionRepo;
        private Controllers.Location locationRepo;
        public Context Context;

        public UnitOfWork(Context db)
        {
            Context = db;
        }

        public ItemController ItemRepo
        {
            get
            {
                if (itemRepository == null)
                {
                    itemRepository = new ItemController(Context);
                }
                return itemRepository;
            }
        }
        public CustomerController CustomerRepo
        {
            get
            {
                if (customerRepo == null)
                {
                    customerRepo = new CustomerController(Context);
                }
                return customerRepo;
            }
        }

        public DocumentController RangeRepo
        {
            get
            {
                if (rangeRepo == null)
                {
                    rangeRepo = new DocumentController(Context);
                }
                return rangeRepo;
            }
        }

        public InventoryController InventoryRepo
        {
            get
            {
                if (inventoryRepo == null)
                {
                    inventoryRepo = new InventoryController(Context);
                }
                return inventoryRepo;
            }
        }

        public VatController VatRepo
        {
            get
            {
                if (vatRepo == null)
                {
                    vatRepo = new VatController(Context);
                }
                return vatRepo;
            }
        }
        public Controllers.Account AccountRepo
        {
            get
            {
                if (accountRepo == null)
                {
                    accountRepo = new Controllers.Account(Context);
                }
                return accountRepo;
            }
        }

        public ItemCategoryController ItemCategoryRepo
        {
            get
            {
                if (itemcategoryRepo == null)
                {
                    itemcategoryRepo = new ItemCategoryController(Context);
                }
                return itemcategoryRepo;
            }
        }
        public Sales SaleRepo
        {
            get
            {
                if (salesRepo == null)
                {
                    salesRepo = new Sales(Context);
                }
                return salesRepo;
            }
        }
        public Controllers.PaymentType PaymentTypeRepo
        {
            get
            {
                if (paymentTypeRepo == null)
                {
                    paymentTypeRepo = new Controllers.PaymentType(Context);
                }
                return paymentTypeRepo;
            }
        }

        public Controllers.PaymentSchedual PaymentSchedualRepo
        {
            get
            {
                if (paymentSchedualRepo == null)
                {
                    paymentSchedualRepo = new Controllers.PaymentSchedual(Context);
                }
                return paymentSchedualRepo;
            }
        }

        public Controllers.Session SessionRepo
        {
            get
            {
                if (sessionRepo == null)
                {
                    sessionRepo = new Controllers.Session(Context);
                }
                return sessionRepo;
            }
        }

        public Controllers.Location LocationRepo
        {
            get
            {
                if (locationRepo == null)
                {
                    locationRepo = new Controllers.Location(Context);
                }
                return locationRepo;
            }
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
