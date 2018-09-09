using System;
using System.Collections.Generic;

namespace Core.Models.Base
{
    public class Vat
    {
        public Vat()
        {
            details = new List<VatDetail>();
        }

        public int id { get; set; }
        public int cloudID { get; set; }

        public string name { get; set; }
        public List<VatDetail> details { get; set; }
    }
}
