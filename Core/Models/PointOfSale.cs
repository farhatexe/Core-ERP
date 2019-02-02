using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class PointOfSale : BaseClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        [DataMember]
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        [DataMember]
        public virtual Company company { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        [DataMember]
        public virtual Location location { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the default type of the payment.
        /// </summary>
        /// <value>The default type of the payment.</value>
        [DataMember]
        
        public virtual PaymentType defaultPaymentType { get; set; }

        /// <summary>
        /// Gets or sets the default type of the account.
        /// </summary>
        /// <value>The default type of the account.</value>
       
        [DataMember]
        
        public virtual Account defaultAccount { get; set; }
       

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> prefill payment.
        /// Prefill amount paid with the exact due amount
        /// </summary>
        /// <value><c>true</c> if prefill payment; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool prefillPayment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> is restaurant.
        /// </summary>
        /// <value><c>true</c> if is restaurant; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool isRestaurant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> uses touch screen.
        /// </summary>
        /// <value><c>true</c> if uses touch screen; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool usesTouchScreen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> is tax inclusive.
        /// </summary>
        /// <value><c>true</c> if is tax inclusive; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool isTaxInclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> has loyalty program.
        /// </summary>
        /// <value><c>true</c> if has loyalty program; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool hasLoyaltyProgram { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> has cash control.
        /// </summary>
        /// <value><c>true</c> if has cash control; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool hasCashControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.PointOfSale"/> default payment condition.
        /// </summary>
        /// <value><c>true</c> if default payment condition; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool defaultPaymentCondition { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        [DataMember]
        public DateTime? createdAt { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        [DataMember]
        public DateTime? updatedAt { get; set; }

        /// <summary>
        /// Gets or sets the deleted at.
        /// </summary>
        /// <value>The deleted at.</value>
        [DataMember]
        public DateTime? deletedAt { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        [DataMember]
        public int action { get; set; }

        public virtual ObservableCollection<Session> sessions { get; set; }
    }
}
