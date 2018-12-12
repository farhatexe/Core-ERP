using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class ItemPromotion : BaseClass
    {
        /// <summary>
        /// Core's logic will utilize Input Types, such as Category, Product, and Location 
        /// and in conjunction with OutputTypes will calculate the best posible price for the customer.
        /// 
        /// The InputTypes are the categories, products, or locations from selling in which to apply the price rule.
        /// </summary>
        public enum InputTypes 
        {
            /// <summary>
            /// Rule applies to all Products and Locations
            /// </summary>
            General,

            /// <summary>
            /// Rule applies to specific Category
            /// </summary>
            OnCategory,

            /// <summary>
            /// Rule applies to specific Product
            /// </summary>
            OnProduct,

            /// <summary>
            /// The on tag.
            /// </summary>
            OnTag,

            /// <summary>
            /// The on customer.
            /// </summary>
            OnCustomer,

            /// <summary>
            /// The on membership.
            /// </summary>
            OnMembershipTier,

            /// <summary>
            /// The type of the on payment.
            /// </summary>
            OnPaymentType,

            /// <summary>
            /// The on payment condition.
            /// </summary>
            OnPaymentCondition
        }

        /// <summary>
        /// Core's logic will use OutputTypes to calculate which 
        /// </summary>
        public enum OutputTypes 
        { 
            /// <summary>
            /// Automatic set percentage on an input.
            /// </summary>
            Discount,

            /// <summary>
            /// If Quantity reaches threshold, apply set percentage on input.
            /// </summary>
            DiscountOnQuantity,

            /// <summary>
            /// If Quantity reaches threshold, give free gift (specified in OutputReference). This will add a new Line to the Order.
            /// </summary>
            GiftOnQuantity,

            /// <summary>
            /// If Order Total reaches threshold, give free gift (specified in OutputReference). This will add a new Line to the Order.
            /// </summary>
            GiftOnTotal
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int? cloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the start date. Null StartDate equals to inmediate start to rule.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime? startDate { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the end date. Null EndDate equals to never ending rule.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime? endDate { get; set; }

        /// <summary>
        /// Gets or sets the on location.
        /// </summary>
        /// <value>The on location.</value>
        public int? onLocationCloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the type of the input.
        /// </summary>
        /// <value>The type of the input.</value>
        public InputTypes inputType { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the input reference. If your customer where to buy 1 box of milk, on which we would then either apply a discount or free gift.
        /// The Item.Id for the box of milk would be your InputReference.
        /// </summary>
        /// <value>The input reference.</value>
        public int inputReference { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the input value. This equals to either quantity or value of the Input Reference. This would be considered the condition on which to apply the rule. For instance, if you buy 1 box of milk, the quantity one 1 equals to your value.
        /// </summary>
        /// <value>The input value.</value>
        public decimal inputValue { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the input value max.
        /// </summary>
        /// <value>The input value max.</value>
        public decimal inputValueMax { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the type of the output.
        /// </summary>
        /// <value>The type of the output.</value>
        public OutputTypes outputType { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the output reference (nullable). Output reference is used to reference an aditional item, not included in the Input Reference. Such as a free gift.
        /// Example: If buying 1 carton of milk you get 1 box of cookies. The Output Reference would be: the Item.Id belonging to the cookie.
        /// </summary>
        /// <value>The output reference.</value>
        public int? outputReference { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the output value. Output Value can be a discount percentage or quantity of items to be given as gift.
        /// </summary>
        /// <value>The output value.</value>
        public decimal outputValue { get; set; }

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
        public int action { get; set; }


        public List<OrderDetail> Details { get; set; }
    }
}
