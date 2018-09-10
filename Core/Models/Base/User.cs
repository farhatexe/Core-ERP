using System;
namespace Core.Models.Base
{
    public class User
    {
        public User()
        {
            
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Pin { get; set; }
        public string ApiKey { get; set; }
    }
}
