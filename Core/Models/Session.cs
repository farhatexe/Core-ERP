using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.ObjectModel;

namespace Core.Models
{
    public class Session : BaseClass
    {
        public Session()
        {
            movements = new ObservableCollection<AccountMovement>();
            orders = new ObservableCollection<Order>();
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
        public virtual PointOfSale PointOfSale { get; set; }

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

        [DataMember]
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
        public virtual ObservableCollection<AccountMovement> movements { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        [DataMember]
        public virtual ObservableCollection<Order> orders { get; set; }

        private decimal _CurrentEndingBalance;
        [NotMapped]
        public decimal CurrentEndingBalance
        {
            get
            {
                _CurrentEndingBalance = startingBalance + movements.
                   Where(x => x.type == Types.Transaction && x.paymentType.behavior == PaymentType.Behaviors.Normal)
                   .Sum(x => x.credit - x.debit);
                return _CurrentEndingBalance;
            }
            set
            {
                _CurrentEndingBalance = value;
            }
        }



        decimal _SalesBalance;
        [NotMapped]
        public decimal salesBalance
        {
            get
            {
                _SalesBalance = movements.
                    Where(x => x.type == Types.Transaction && x.paymentType.behavior == PaymentType.Behaviors.Normal)
                    .Sum(x => x.credit - x.debit);
                return _SalesBalance;
            }
            set
            {
                _SalesBalance = value;
            }
        }


        [NotMapped]
        public decimal ClosingChange { get; set; }

        private string _comment;
        [NotMapped]
        public string comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
    }
}