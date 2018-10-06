using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class PointOfSale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company Company { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location Location { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price list.
        /// </summary>
        /// <value>The price list.</value>
        public ItemPriceList PriceList { get; set; }

        /// <summary>
        /// Gets or sets the default type of the payment.
        /// </summary>
        /// <value>The default type of the payment.</value>
        public PaymentType DefaultPaymentType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> prefill payment.
        /// Prefill amount paid with the exact due amount
        /// </summary>
        /// <value><c>true</c> if prefill payment; otherwise, <c>false</c>.</value>
        public bool PrefillPayment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> is restaurant.
        /// </summary>
        /// <value><c>true</c> if is restaurant; otherwise, <c>false</c>.</value>
        public bool IsRestaurant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> uses touch screen.
        /// </summary>
        /// <value><c>true</c> if uses touch screen; otherwise, <c>false</c>.</value>
        public bool UsesTouchScreen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> is tax inclusive.
        /// </summary>
        /// <value><c>true</c> if is tax inclusive; otherwise, <c>false</c>.</value>
        public bool IsTaxInclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> has loyalty program.
        /// </summary>
        /// <value><c>true</c> if has loyalty program; otherwise, <c>false</c>.</value>
        public bool HasLoyaltyProgram { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> has cash control.
        /// </summary>
        /// <value><c>true</c> if has cash control; otherwise, <c>false</c>.</value>
        public bool HasCashControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> default payment condition.
        /// </summary>
        /// <value><c>true</c> if default payment condition; otherwise, <c>false</c>.</value>
        public bool DefaultPaymentCondition { get; set; }
    }
}
