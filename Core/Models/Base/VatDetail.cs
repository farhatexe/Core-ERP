using System;
namespace Core.Models.Base
{
    public class VatDetail
    {
        public VatDetail()
        {
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public string Name { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Percentage { get; set; }
    }
}
