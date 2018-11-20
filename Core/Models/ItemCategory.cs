using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ItemCategory
    {
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public ItemCategory Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.ItemCategory"/> is grouping.
        /// </summary>
        /// <value><c>true</c> if is grouping; otherwise, <c>false</c>.</value>
        public bool IsGrouping { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<ItemCategory> Children { get; set; }
    }
}
