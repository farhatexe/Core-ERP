using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class InventoryDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        public Inventory inventory { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item item { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the quantity system in that location.
        /// </summary>
        /// <value>The qty system.</value>
        public decimal qtySystem { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the quantity counted by the user in that location.
        /// </summary>
        /// <value>The qty counted.</value>
        public decimal? qtyCounted { get; set; }
        [DataMember]
        /// <summary>
        /// Gets the difference between Quantity System and Quantity Counted.
        /// </summary>
        /// <value>The difference.</value>
        public decimal difference { get => (qtySystem - (decimal)qtyCounted); }
        [DataMember]
        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal cost { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string comment { get; set; }
    }
}
