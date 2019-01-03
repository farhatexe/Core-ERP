using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Documents are used to generate rages for Invoices or other internal documents
    /// </summary>
    public class Document : BaseClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        [DataMember]
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public virtual Company company { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public virtual Location location { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the point of sale.
        /// </summary>
        /// <value>The point of sale.</value>
        public virtual PointOfSale pointOfSale { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>The module.</value>
        public int module { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the type. 
        /// </summary>
        /// <value>The type.</value>
        public string type { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the designUrl. 
        /// </summary>
        /// <value>The designUrl.</value>
        public string designUrl { get; set; }

        /// <summary>
        /// Gets or sets the number template.
        /// </summary>
        /// <value>The number template.</value>
        [DataMember]
        public string numberTemplate { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the mask. 
        /// </summary>
        /// <value>The mask.</value>
        public string mask { get; set; }

        public virtual ObservableCollection<Range> details { get; set; }

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
    }
}
