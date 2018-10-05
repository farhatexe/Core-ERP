using Core.Models;
using Core.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class UnitOfWork
    {
        private Item _itemRepo;
        private Customer _customerRepo;
        private RangeRepository _rangeRepo;
        private Context _db;

        public UnitOfWork(Context db)
        {
            _db = db;
        }

        public Item ItemRepo
        {
            get
            {
                if (_itemRepo == null)
                {
                    _itemRepo = new Item(_db);
                }
                return _itemRepo;
            }
        }
        public Customer CustomerRepo
        {
            get
            {
                if (_customerRepo == null)
                {
                    _customerRepo = new Customer(_db);
                }
                return _customerRepo;
            }
        }

        public RangeRepository RangeRepo
        {
            get
            {
                if (_rangeRepo == null)
                {
                    _rangeRepo = new RangeRepository(_db);
                }
                return _rangeRepo;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
