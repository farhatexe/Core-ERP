using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Items are products or services you commercialize.
    /// </summary>
    [DataContract]
    public class Item : BaseClass
    {
        public Item()
        {
            isPrivate = false;
            isActive = true;
            ItemMovements = new List<ItemMovement>();
        }

        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        [DataMember]
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        [DataMember]
        public Company company { get; set; }

        /// <summary>
        /// Gets or sets the global item cloud identifier.
        /// </summary>
        /// <value>The global item cloud identifier.</value>
        [DataMember]
        public int? globalId { get; set; }

        /// <summary>
        /// Gets or sets the vat cloud identifier.
        /// </summary>
        /// <value>The vat cloud identifier.</value>
        [DataMember]
        public int? vatCloudId { get; set; }

        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>The vat.</value>
        [DataMember]
        public Vat vat { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public int? categoryCloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public ItemCategory category { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the sku.
        /// </summary>
        /// <value>The sku.</value>
        [DataMember]
        public string sku { get; set; }

        /// <summary>
        /// Gets or sets the bar code.
        /// </summary>
        /// <value>The bar code.</value>
        [DataMember]
        public string barCode { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        /// <value>The short description.</value>
        [DataMember]
        public string shortDescription { get; set; }

        /// <summary>
        /// Gets or sets the long description.
        /// </summary>
        /// <value>The long description.</value>
        [DataMember]
        public string longDescription { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        [DataMember]
        public string imageUrl { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string currencyCode { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public decimal price 
        {
            get 
            {
                if (_price == 0)
                {
                    if (category != null && category.margin != null)
                    {
                        return (cost * (category.margin ?? 0 + 1));
                    }
                    else if (company != null && company.globalMargin != null)
                    { 
                        return (cost * (company.globalMargin ?? 0 + 1));
                    }
                }

                return _price;
            }
            set 
            { 
                _price = value;

            }
        }
        private decimal _price;

        [DataMember]
        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal cost { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.Item"/> weigh with scale.
        /// </summary>
        /// <value><c>true</c> if weigh with scale; otherwise, <c>false</c>.</value>
        public bool weighWithScale { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>The weight.</value>
        public decimal? weight { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public decimal? volume { get; set; }

        /// <summary>
        /// Gets or sets the is private.
        /// </summary>
        /// <value>The is private.</value>
        [DataMember]
        public bool isPrivate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Cognitivo.API.Models.Item"/> is stockable.
        /// </summary>
        /// <value><c>true</c> if is stockable; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool isStockable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Cognitivo.API.Models.Item"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool isActive { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>

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

        [DataMember]
        public List<ItemMovement> ItemMovements { get; set; }

        public List<OrderDetail> orderDetails { get; set; }

        public List<Inventory> Inventory { get; set; }
    }
}
