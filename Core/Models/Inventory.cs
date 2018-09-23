using System;
namespace Core.Models
{
    public class Inventory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.Models.InventoryDetail"/> class.
        /// </summary>
        public Inventory()
        {
            Date = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location Location { get; set; }

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
