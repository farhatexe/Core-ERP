using System;
namespace Core.Models
{
    public class VatDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
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
        /// Gets or sets the coefficient.
        /// </summary>
        /// <value>The coefficient.</value>
        public decimal Coefficient { get; set; }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>The percentage.</value>
        public decimal Percentage { get; set; }
    }
}
