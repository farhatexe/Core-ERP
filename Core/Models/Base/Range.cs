using System;
namespace Core.Models.Base
{
    public class Range
    {
        public Range()
        {
            
        }

        public int Id { get; set; }
        public int CloudID { get; set; }

        public int startingValue { get; set; }
        public int currentValue { get; set; }
        public int endingValue { get; set; }

        public string template { get; set; }
        public string mask { get; set; }

        public string code { get; set; }
        public DateTime expiryDate { get; set; }
    }
}
