using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class Range
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        [DataMember]
        public int cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        [DataMember]
        public Company company { get; set; }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        [DataMember]
        public int currentValue { get; set; }

        /// <summary>
        /// Gets or sets the ending value.
        /// </summary>
        /// <value>The ending value.</value>
        [DataMember]
        public int endValue { get; set; }

        /// <summary>
        /// Gets or sets the code. Certain countries or companies may require each range to be signed by an identifier code.
        /// </summary>
        /// <value>The code.</value>
        [DataMember]
        public string code { get; set; }

        /// <summary>
        /// Gets or sets the expiry date. Null value equals to no expiration date.
        /// </summary>
        /// <value>The expiry date.</value>
        [DataMember]
        public DateTime? expiryDate { get; set; }

        /// <summary>
        /// Gets or sets the expiry date. Null value equals to no expiration date.
        /// </summary>
        /// <value>The expiry date.</value>
        [DataMember]
        public DateTime? startDate { get; set; }

        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        [DataMember]
        public List<Order> orders { get; set; }

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
    }
}
