using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Syncronization
    {
        public enum SyncTypes { Upload, Download }

        public Syncronization()
        {
            Date = DateTime.Now;
            SyncType = SyncTypes.Upload;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// 
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public SyncTypes SyncType { get; set; }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public string Object { get; set; }


    }
}
