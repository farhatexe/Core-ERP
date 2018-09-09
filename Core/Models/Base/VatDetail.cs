using System;
namespace Core.Models.Base
{
    public class VatDetail
    {
        public VatDetail()
        {
        }

        public int id { get; set; }
        public int cloudID { get; set; }

        public string name { get; set; }
        public decimal coefficient { get; set; }
        public decimal percentage { get; set; }
    }
}
