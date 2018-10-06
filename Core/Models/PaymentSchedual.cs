using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Models
{
   public class PaymentSchedual
    {
        /// <summary>
        /// Getor Sers Id 
        /// </summary>
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int localId { get; set; }
        /// <summary>
        /// Gets or Sets the Order.
        /// </summary>
        /// <value>The Order</value>
        public Order Order { get; set; }
        // <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the debit.
        /// </summary>
        /// <value>The debit.</value>
        public decimal Debit { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>The credit.</value>
        public decimal Credit { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }
    }
}
