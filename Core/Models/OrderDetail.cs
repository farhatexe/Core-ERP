using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Order detail handle's item, quantity, and price of what is being bought or sold.
    /// </summary>
    public class OrderDetail : BaseClass
    {
        public OrderDetail()
        {
            quantity = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int? cloudId { get; set; }

        private Order _order;
        [DataMember]
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public virtual Order order {
            get => _order;
            set
            {
                _order = value;
                _order.RaisePropertyChanged("total");
            }
        }

        [DataMember]
        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>The vat.</value>
        public virtual Vat vat { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the promotion.
        /// </summary>
        /// <value>The promotion.</value>
        public virtual ItemPromotion promotion { get; set; }

        [DataMember]
        private Item _item;
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public virtual Item item
        {
            get => _item;
            set
            {
                _item = value;
                price = _item.price;
                itemDescription = itemDescription ?? _item.name;
            }
        }

        [DataMember]
        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        /// <value>The item description.</value>
        public string itemDescription { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal cost { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        private decimal _quantity;
        public decimal quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                 RaisePropertyChanged("quantity");
                RaisePropertyChanged("subTotal");
                RaisePropertyChanged("subTotalVat");
                //if (order!=null)
                //{
                //    order.RaisePropertyChanged("total");
                //}
              
            }
        }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>The discount.</value>
        [DataMember]
        public decimal discount { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        [DataMember]
        public decimal price
        {
            get => _price;
            set
            {
                discount = discount == 0 ? value : discount;

                _price = value;
                RaisePropertyChanged("priceVat");
                RaisePropertyChanged("subTotal");
                RaisePropertyChanged("subTotalVat");
                //if (order != null)
                //{
                //    order.RaisePropertyChanged("total");
                //}
            }
        }
        private decimal _price;

        /// <summary>
        /// Gets the price vat.
        /// </summary>
        /// <value>The price vat.</value>
        private decimal _priceVat;
        [NotMapped]
        public decimal priceVat
        {
            get
            {
                if (vat != null)
                {
                    decimal finalCoefficient = 0;
                    foreach (VatDetail item in vat.details)
                    {
                        finalCoefficient += item.percentage * item.coefficient;
                    }

                    return price + (price * finalCoefficient);
                }
                else
                {
                    return price;
                }
            }
            set
            {
                if (_priceVat != value)
                {
                    if (value == 0)
                    {
                        _priceVat = value;
                        RaisePropertyChanged("priceVat");
                    }
                    else
                    {
                        _priceVat = value;
                        RaisePropertyChanged("priceVat");
                        if (vat != null)
                        {
                            price = value / (1 + vat.details.Sum(x=>x.coefficient * x.percentage));
                            RaisePropertyChanged("price");
                        }
                        else
                        {
                            price = value / 1;
                            RaisePropertyChanged("price");
                        }
                    }
                    order.RaisePropertyChanged("total");
                }

            }
        }

        /// <summary>
        /// Gets the sub total.
        /// </summary>
        /// <value>The sub total.</value>
        private decimal _subTotal;
        [NotMapped]
        public decimal subTotal
        {
            get
            {
                return price * quantity;
            }
            set
            {
                _subTotal = value;
            }
        }

        /// <summary>
        /// Gets the sub total vat.
        /// </summary>
        /// <value>The sub total vat.</value>
        decimal _subTotalVat;
        [NotMapped]
        public decimal subTotalVat
        {
            get
            {
                return priceVat * quantity;
            }
            set
            {
                _subTotalVat = value;
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [NotMapped]
        public Message.Warning? message { get; set; }

        [NotMapped]
        public bool QuantityUpdated { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime? createdAt { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime? updatedAt { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the deleted at.
        /// </summary>
        /// <value>The deleted at.</value>
        public DateTime? deletedAt { get; set; }

        [NotMapped]
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public Enums.Action action { get; set; }

        [NotMapped]
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public bool isSelected { get; set; }
    }
}
