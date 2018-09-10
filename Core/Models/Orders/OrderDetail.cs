using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Orders
{
    public class OrderDetail
    {
        private Inventories.Item _item;

        public OrderDetail()
        {
            Quantity = 1;
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public Inventories.Item Item { 
            get => _item; 
            set 
            {
                _item = value;
                Price = _item.Price;
            } 
        }

        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public decimal PriceVat
        {
            get
            {
                return Price * Quantity;
            }
        }

        [NotMapped]
        public decimal SubTotal {
            get {
                return Price * Quantity;
            }
        }

        [NotMapped]
        public decimal SubTotalVat {
            get {
                return Price * Quantity;
            }
        }
    }
}
