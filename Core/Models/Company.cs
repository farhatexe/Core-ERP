using System;
namespace Core.Models
{
    public class Company
    {
        /// <summary>
        /// Local ID used for 
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        public string SlugCognitivo { get; set; }

        public string Name { get; set; }
        public string TaxID { get; set; }

        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public string Currency { get; set; }
    }
}
