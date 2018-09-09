using System;
namespace Core.Models.Inventories
{
    public class Item
    {
        
        public string name { get; set; }
        public string sku { get; set; }
        public string barCode { get; set; }

        public string currency { get; set; }
        public decimal price { get; set; }
    }
}
