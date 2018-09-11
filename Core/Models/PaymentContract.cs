using System;
using System.Collections.Generic;

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
