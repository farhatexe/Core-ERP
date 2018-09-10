using System;
using System.Collections.Generic;

namespace Core.Models.Base
{
    public class Vat
    {
        public Vat()
        {
            Details = new List<VatDetail>();
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public string Name { get; set; }
        public List<VatDetail> Details { get; set; }
    }
}
