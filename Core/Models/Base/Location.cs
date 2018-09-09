using System;
namespace Core.Models.Base
{
    public class Location
    {
        public Location() { }

        public int id { get; set; }
        public int cloudID { get; set; }

        public string name { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }

        public Vat vat { get; set; }
    }
}
