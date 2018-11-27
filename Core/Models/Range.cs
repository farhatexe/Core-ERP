using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Range
    {
        public Range()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }
        public int cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company company { get; set; }

 

       

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        public int currentValue { get; set; }

        /// <summary>
        /// Gets or sets the ending value.
        /// </summary>
        /// <value>The ending value.</value>
        public int endValue { get; set; }

     
        /// <summary>
        /// Gets or sets the code. Certain countries or companies may require each range to be signed by an identifier code.
        /// </summary>
        /// <value>The code.</value>
        public string code { get; set; }

        /// <summary>
        /// Gets or sets the expiry date. Null value equals to no expiration date.
        /// </summary>
        /// <value>The expiry date.</value>
        public DateTime? expiryDate { get; set; }

        /// <summary>
        /// Gets or sets the expiry date. Null value equals to no expiration date.
        /// </summary>
        /// <value>The expiry date.</value>
        public DateTime? startDate { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime createdAt { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime updatedAt { get; set; }


        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public int action { get; set; }

        public List<Order> orders { get; set; }
    }
}
