using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class Company
    {
        /// <summary>
        /// Local ID used for 
        /// </summary>
        /// <value>The identifier.</value>
        /// 
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
        [DataMember]
        /// <summary>
        /// Gets or sets the tax identifier.
        /// </summary>
        /// <value>The tax identifier.</value>
        public string taxId { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string address { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the telephone.
        /// </summary>
        /// <value>The telephone.</value>
        public string telephone { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string email { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string currency { get; set; }

        public List<Vat> vats { get; set; }
        public List<PointOfSale> pointOfSales { get; set; }
        public List<Range> ranges { get; set; }
        public List<Customer> contacts { get; set; }
        public List<Location> locations { get; set; }
        public List<PaymentType> paymentTypes { get; set; }
        public List<ItemMovement> itemMovements { get; set; }
        public List<Order> orders { get; set; }
    }
}
