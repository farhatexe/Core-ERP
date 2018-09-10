using System;
namespace Core.Models.Inventories
{
    public class Item
    {
        public int Id { get; set; }
        public int CloudID { get; set; }

        public string Name { get; set; }
        public string Sku { get; set; }
        public string BarCode { get; set; }

        public string Currency { get; set; }
        public decimal Price { get; set; }
    }
}
