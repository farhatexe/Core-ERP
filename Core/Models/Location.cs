using System;
namespace Core.Models
{
    public class Location
    {
        public Location() { }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public Vat Vat { get; set; }
    }
}
