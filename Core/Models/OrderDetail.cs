using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class OrderDetail
    {
        private Inventories.Item _item;

        public OrderDetail()
        {
            Quantity = 1;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public Order Order { get; set; }

        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>The vat.</value>
        public Base.Vat Vat { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Inventories.Item Item { 
            get => _item; 
            set 
            {
                _item = value;
                Price = _item.Price;
            } 
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets the price vat.
        /// </summary>
        /// <value>The price vat.</value>
        [NotMapped]
        public decimal PriceVat
        {
            get
            {
                return Price * Vat.Coefficient;
            }
        }

        /// <summary>
        /// Gets the sub total.
        /// </summary>
        /// <value>The sub total.</value>
        [NotMapped]
        public decimal SubTotal {
            get {
                return Price * Quantity;
            }
        }

        /// <summary>
        /// Gets the sub total vat.
        /// </summary>
        /// <value>The sub total vat.</value>
        [NotMapped]
        public decimal SubTotalVat {
            get {
                return PriceVat * Quantity;
            }
        }
    }
}
