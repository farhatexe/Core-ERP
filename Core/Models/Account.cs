using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Account.
    /// </summary>
    /// 
    
    public class Account
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
        [DataMember]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string currency { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public string number { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Cognitivo.API.Models.Item"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool isActive { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        [DataMember]
        public DateTime createdAt { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        [DataMember]
        public DateTime updatedAt { get; set; }





        public List<AccountMovement> accountMovements { get; set; }
    }
}
