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
        private Context Context;

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

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
