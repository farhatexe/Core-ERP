using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Models
{
    public class Session
    {
        public Session()
        {
            Movements = new List<AccountMovement>();
            Orders = new List<Order>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the point of sale.
        /// </summary>
        /// <value>The point of sale.</value>
        public PointOfSale PointOfSale { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the starting balance.
        /// </summary>
        /// <value>The starting balance.</value>
        public decimal StartingBalance { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the ending balance.
        /// </summary>
        /// <value>The ending balance.</value>
        public decimal EndingBalance { get; set; }

        /// <summary>
        /// Gets or sets the movements.
        /// </summary>
        /// <value>The movements.</value>
        public List<AccountMovement> Movements { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        public List<Order> Orders { get; set; }

        [NotMapped]
        public decimal CurrentEndingBalance{
            get {
                return StartingBalance + Orders.Sum(x => x.Total * x.CurrencyRate);
            }
        }
    }
}