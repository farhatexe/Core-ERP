using System;
namespace Core.Models
{
    public class Range
    {
        public Range()
        {
            
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public int StartingValue { get; set; }
        public int CurrentValue { get; set; }
        public int EndingValue { get; set; }

        public string Template { get; set; }
        public string Mask { get; set; }

        public string Code { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
