﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class User : BaseClass
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the passcode.
        /// </summary>
        /// <value>The passcode.</value>
        public string passcode { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        public string apiKey { get; set; }

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
    }
}
