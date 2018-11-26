﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Contact.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int? cloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>The name.</value>
        public string alias { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the tax identifier.
        /// </summary>
        /// <value>The tax identifier.</value>
        public string taxId { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string address { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string email { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the telephone.
        /// </summary>
        /// <value>The telephone.</value>
        public string telephone { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the lead time.
        /// </summary>
        /// <value>The lead time.</value>
        public int? leadTime { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the credit limit.
        /// </summary>
        /// <value>The credit limit.</value>
        public int? creditLimit { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the contact ref.
        /// </summary>
        /// <value>The credit limit.</value>
        public int? contractRef { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime createdAt { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime updatedAt { get; set; }
     
        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public int action { get; set; }

        public List<Order> orders { get; set; }
    }
}
