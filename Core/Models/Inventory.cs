using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class Inventory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.Models.InventoryDetail"/> class.
        /// </summary>
        public Inventory()
        {
            date = DateTime.Now;
           
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }
        [DataMember]
        public int? cloudId { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime date { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location location { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item item { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the locationId.
        /// </summary>
        /// <value>The locationId.</value>
        public int locationId { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the itemId.
        /// </summary>
        /// <value>The itemId.</value>
        public int itemId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal cost { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the systemQuantity.
        /// </summary>
        /// <value>The systemQuantity.</value>

        public decimal systemQuantity { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the actualQuantity.
        /// </summary>
        /// <value>The actualQuantity.</value>

        public decimal actualQuantity { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>

        public string comment { get; set; }
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



    }
}
