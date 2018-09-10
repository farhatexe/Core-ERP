﻿using System;
namespace Core.Models.Orders
{
    public class Contact
    {
        public Contact()
        {
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public string Name { get; set; }
        public string TaxID { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
