using System;
namespace Core.Models.Inventories
{
    public class ItemMovement
    {
        public ItemMovement()
        {
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public Item Item { get; set; }
        public DateTime Date { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public string Comment { get; set; }
    }
}
