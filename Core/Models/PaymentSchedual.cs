﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Models
{
   public class PaymentSchedual : BaseClass
    {
        /// <summary>
        /// Getor Sers Id 
        /// </summary>
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int localId { get; set; }

        [DataMember]
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or Sets the Order.
        /// </summary>
        /// <value>The Order</value>
        [DataMember]
        public virtual Order order { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        [DataMember]
        public DateTime? date { get; set; }

        /// <summary>
        /// Gets or sets the amount owed.
        /// </summary>
        /// <value>The amount owed.</value>
        [DataMember]
        public decimal amountOwed { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [DataMember]
        public string comment { get; set; }

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
        /// <summary>
        /// Get or Sets Movements
        /// </summary>
        public virtual ObservableCollection<AccountMovement> movements { get; set; }
    }
}
