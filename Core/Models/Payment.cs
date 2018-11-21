using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Models
{
   public class Payment
    {
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int localId { get; set; }

        /// <summary>
        /// Gets or Sets the Order.
        /// </summary>
        /// <value>The Order</value>
        public Order order { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime date { get; set; }



        public int schedualId { get; set; }
        /// <summary>
        /// Gets or sets the amount owed.
        /// </summary>
        /// <value>The amount owed.</value>
        public decimal amountOwed { get; set; }

        public PaymentType paymentType { get; set; }

     

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string comment { get; set; }
    }
}
