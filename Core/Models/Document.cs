using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    public class Document
    {
        public Document()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }
        [DataMember]
        public int? cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company company { get; set; }


        [DataMember]
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location location { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the point of sale.
        /// </summary>
        /// <value>The point of sale.</value>
        public PointOfSale pointOfSale { get; set; }


        [DataMember]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public int name { get; set; }
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
        [DataMember]
        /// <summary>
        /// Gets or sets the codeTemplate. 
        /// </summary>
        /// <value>The codeTemplate.</value>
        public string codeTemplate { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the mask. 
        /// </summary>
        /// <value>The mask.</value>
        public string mask { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime createdAt { get; set; }
        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime updatedAt { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public int action { get; set; }

        public List<Range> details { get; set; }
    }
}
