using System;
namespace Core.Models.Orders
{
    public class Contact
    {
        public Contact()
        {
        }

        public int id { get; set; }
        public int cloudID { get; set; }

        public string name { get; set; }
        public string taxid { get; set; }

        public string address { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
    }
}
