using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class PaymentContractDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the contract.
        /// </summary>
        /// <value>The contract.</value>
        public PaymentContract PaymentContract { get; set; }

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
