using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    /// <summary>
    /// Order detail handle's item, quantity, and price of what is being bought or sold.
    /// </summary>
    public class OrderDetail : INotifyPropertyChanged
    {
        private Item _item;

        public OrderDetail()
        {
            quantity = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public Order order { get; set; }

        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>The vat.</value>
        public Vat vat { get; set;  }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item item { 
            get => _item; 
            set 
            {
                _item = value;
                price = _item.price;
                itemDescription = itemDescription ?? _item.name;
            } 
        }

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        /// <value>The item description.</value>
        public string itemDescription { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal cost { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public decimal quantity { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public decimal price { get; set; }

        /// <summary>
        /// Gets the price vat.
        /// </summary>
        /// <value>The price vat.</value>
        [NotMapped]
        public decimal priceVat
        {
            get
            {
                return price * vat.coefficient;
            }
        }

        /// <summary>
        /// Gets the sub total.
        /// </summary>
        /// <value>The sub total.</value>
        [NotMapped]
        public decimal subTotal {
            get {
                return price * quantity;
            }
        }

        /// <summary>
        /// Gets the sub total vat.
        /// </summary>
        /// <value>The sub total vat.</value>
        [NotMapped]
        public decimal subTotalVat {
            get {
                return priceVat * quantity;
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [NotMapped]
        public Message.Warning? message { get; set; }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    }
}
