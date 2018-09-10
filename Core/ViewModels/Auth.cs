using System;
using System.Collections.Generic;

namespace Core.ViewModels
{
    public class Auth
    {
        public Models.Base.User User;
        public Models.Base.Company Company;

        public Auth(string User, string Pin)
        {  
            
        }

        public List<Models.Base.Company> Companies()
        {
            return null;
        }
    }
}
