using System;
using System.Collections.Generic;

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
            Details = new List<InventoryDetail>();
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public List<Models.InventoryDetail> Details { get; set; }
    }
}
