using System;
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
        public int Id { get; set; }
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company Company { get; set; }

        /// <summary>
        /// Gets or sets the module name to be used.
        /// </summary>
        /// <value>The module.</value>
        public string Module { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location Location { get; set; }

        /// <summary>
        /// Gets or sets the point of sale.
        /// </summary>
        /// <value>The point of sale.</value>
        public PointOfSale PointOfSale { get; set; }

        /// <summary>
        /// Gets or sets the starting value.
        /// </summary>
        /// <value>The starting value.</value>
        public int StartingValue { get; set; }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        public int CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the ending value.
        /// </summary>
        /// <value>The ending value.</value>
        public int EndingValue { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the mask.
        /// </summary>
        /// <value>The mask.</value>
        public string Mask { get; set; }

        /// <summary>
        /// Gets or sets the code. Certain countries or companies may require each range to be signed by an identifier code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the expiry date. Null value equals to no expiration date.
        /// </summary>
        /// <value>The expiry date.</value>
        public DateTime? ExpiryDate { get; set; }
    }
}
