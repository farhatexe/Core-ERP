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
        private RangeController rangeRepo;
        private InventoryController inventoryRepo;
        private VatController vatRepo;
        private Controllers.Account accountRepo;
        private ItemCategoryController itemcategoryRepo;
        private Sales salesRepo;
        private Controllers.PaymentType paymentTypeRepo;
        private Controllers.PaymentSchedual paymentSchedualRepo;
        private Controllers.Session sessionRepo;
        private Controllers.Location locationRepo;
        private Controllers.PaymentContract contractRepo;
        private PointOfSales pointofsaleRepo;
        private Controllers.ItemPromotion itempromotionRepo;
        private Controllers.DocumentController documentRepo;

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

        public RangeController RangeRepo
        {
            get
            {
                if (rangeRepo == null)
                {
                    rangeRepo = new RangeController(Context);
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

        public Controllers.PaymentContract PaymentContractRepo
        {
            get
            {
                if (contractRepo == null)
                {
                    contractRepo = new Controllers.PaymentContract(Context);
                }
                return contractRepo;
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

        public Controllers.PointOfSales PointOfSaleRepo
        {
            get
            {
                if (pointofsaleRepo == null)
                {
                    pointofsaleRepo = new Controllers.PointOfSales(Context);
                }
                return pointofsaleRepo;
            }
        }
        public Controllers.ItemPromotion PromotionRepo
        {
            get
            {
                if (itempromotionRepo == null)
                {
                    itempromotionRepo = new Controllers.ItemPromotion(Context);
                }
                return itempromotionRepo;
            }
        }
        public Controllers.DocumentController DocumentRepo
        {
            get
            {
                if (documentRepo == null)
                {
                    documentRepo = new Controllers.DocumentController(Context);
                }
                return documentRepo;
            }
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
