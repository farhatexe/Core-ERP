using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class PaymentContract
    {
        public PaymentContract()
        {
            Details = new List<PaymentContractDetail>();
        }

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
        /// Gets or sets the details.
        /// </summary>
        /// <value>The details.</value>
        public List<PaymentContractDetail> Details { get; set; }
    }
}
