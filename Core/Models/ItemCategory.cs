﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Models
{
    /// <summary>
    /// Item category.
    /// </summary>
    public class ItemCategory : BaseClass
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int localId { get; set; }

        /// <summary>
        /// Gets or sets the cloud identifier.
        /// </summary>
        /// <value>The cloud identifier.</value>
        [DataMember]
        public int? cloudId { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public virtual ItemCategory parent { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Core.Models.ItemCategory"/> is grouping.
        /// </summary>
        /// <value><c>true</c> if is grouping; otherwise, <c>false</c>.</value>
        public bool group { get; set; }

        /// <summary>
        /// Gets or sets the global margin.
        /// </summary>
        /// <value>The global margin.</value>
        [DataMember]
        public decimal? margin { get; set; }

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

        //[DataMember]
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public virtual ObservableCollection<ItemCategory> children { get; set; }

        public virtual ObservableCollection<Item> items { get; set; }
    }
}
