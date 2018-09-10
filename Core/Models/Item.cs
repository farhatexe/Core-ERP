using System;
namespace Core.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int CloudID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sku.
        /// </summary>
        /// <value>The sku.</value>
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets the bar code.
        /// </summary>
        /// <value>The bar code.</value>
        public string BarCode { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public decimal Price { get; set; }
    }
}
