using System;
namespace Core.Models
{
    public class InventoryDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public Inventory Inventory { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item Item { get; set; }

        /// <summary>
        /// Gets or sets the quantity system in that location.
        /// </summary>
        /// <value>The qty system.</value>
        public decimal QtySystem { get; set; }

        /// <summary>
        /// Gets or sets the quantity counted by the user in that location.
        /// </summary>
        /// <value>The qty counted.</value>
        public decimal QtyCounted { get; set; }

        /// <summary>
        /// Gets the difference between Quantity System and Quantity Counted.
        /// </summary>
        /// <value>The difference.</value>
        public decimal Difference { get => (QtySystem - QtyCounted); }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }
    }
}
