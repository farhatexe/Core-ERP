﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Models
{
    public class Vat
    {
        public Vat()
        {
            details = new List<VatDetail>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        public int cloudId { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public Company company { get; set; }

        /// <summary>
        /// Name for easy recognition.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Read only property that calculates the prorated coeficient from the details, ex= Details.Sum(x => x.Coeficient * x.Percentage).
        /// </summary>
        /// <value>Coeficient of the details</value>
        [NotMapped]
        public decimal coefficient {
            get {
                return details.Sum(x => x.coefficient * x.percentage);
            }
        }

        /// <summary>
        /// List of Details 
        /// </summary>
        /// <value>The details.</value>
        public List<VatDetail> details { get; set; }
    }
}
