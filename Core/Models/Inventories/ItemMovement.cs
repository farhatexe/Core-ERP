using System;
namespace Core.Models.Inventories
{
    public class ItemMovement
    {
        public ItemMovement()
        {
        }

        public int id { get; set; }

        public Item item { get; set; }
        public DateTime date { get; set; }

        public decimal debit { get; set; }
        public decimal credit { get; set; }

        public string comment { get; set; }
    }
}
