using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Gets the Profile
    /// </summary>
    public class Company : BaseClass
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the slug cognitivo.
        /// </summary>
        /// <value>The slug cognitivo.</value>
        public string slugCognitivo { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the tax identifier.
        /// </summary>
        /// <value>The tax identifier.</value>
        [DataMember]
        public string taxId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [DataMember]
        public string address { get; set; }

        /// <summary>
        /// Gets or sets the telephone.
        /// </summary>
        /// <value>The telephone.</value>
        [DataMember]
        public string telephone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [DataMember]
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        [DataMember]
        public string currencyCode { get; set; }

        /// <summary>
        /// Gets or sets the global margin.
        /// </summary>
        /// <value>The global margin.</value>
        [DataMember]
        public decimal? globalMargin { get; set; }

        public virtual ObservableCollection<Vat> vats { get; set; }

        public virtual ObservableCollection<PointOfSale> pointOfSales { get; set; }

        public virtual ObservableCollection<Range> ranges { get; set; }

        public virtual ObservableCollection<Contact> contacts { get; set; }

        public virtual ObservableCollection<Location> locations { get; set; }

        public virtual ObservableCollection<PaymentType> paymentTypes { get; set; }

        public virtual ObservableCollection<ItemMovement> itemMovements { get; set; }

        public virtual ObservableCollection<Order> orders { get; set; }
    }
}
