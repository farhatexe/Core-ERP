using System;
namespace Core.Models.Orders
{
    public class OrderDetail
    {
        private Inventories.Item _item;

        public OrderDetail()
        {
            quantity = 1;
        }

        public int id { get; set; }

        public Inventories.Item item { 
            get => _item; 
            set 
            {
                _item = value;
                price = _item.price;
            } 
        }

        public decimal price { get; set; }
        public decimal quantity { get; set; }
    }
}
