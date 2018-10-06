using Core.Models;
using Core.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class UnitOfWork
    {
        private Item itemRepo;
        private Customer customerRepo;
        private DocumentRange rangeRepo;
        private Inventory inventoryRepo;
        private Context Context;

        public UnitOfWork(Context db)
        {
            Context = db;
        }

        public Item ItemRepo
        {
            get
            {
                if (itemRepo == null)
                {
                    itemRepo = new Item(Context);
                }
                return itemRepo;
            }
        }
        public Customer CustomerRepo
        {
            get
            {
                if (customerRepo == null)
                {
                    customerRepo = new Customer(Context);
                }
                return customerRepo;
            }
        }

        public DocumentRange RangeRepo
        {
            get
            {
                if (rangeRepo == null)
                {
                    rangeRepo = new DocumentRange(Context);
                }
                return rangeRepo;
            }
        }

        public Inventory InventoryRepo
        {
            get
            {
                if (inventoryRepo == null)
                {
                    inventoryRepo = new Inventory(Context);
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
