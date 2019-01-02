using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Core.Models
{
    public class Session : BaseClass
    {
        public Session()
        {
            movements = new List<AccountMovement>();
            orders = new List<Order>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        [DataMember]
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the point of sale.
        /// </summary>
        /// <value>The point of sale.</value>
        public PointOfSale PointOfSale { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime startDate { get; set; }

        /// <summary>
        /// Gets or sets the starting balance.
        /// </summary>
        /// <value>The starting balance.</value>
        public decimal startingBalance { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime? endDate { get; set; }

        /// <summary>
        /// Gets or sets the ending balance.
        /// </summary>
        /// <value>The ending balance.</value>
        public decimal endingBalance { get; set; }

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

        /// <summary>
        /// Gets or sets the movements.
        /// </summary>
        /// <value>The movements.</value>
        [DataMember]
        public List<AccountMovement> movements { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        [DataMember]
        public List<Order> orders { get; set; }

        [NotMapped]
        public decimal CurrentEndingBalance{
            get
            {
                return startingBalance + orders.Sum(x => x.total * x.currencyRate);
            }
        }
    }
}