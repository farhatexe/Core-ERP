using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    /// <summary>
    /// Account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the default currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }
    }
}
