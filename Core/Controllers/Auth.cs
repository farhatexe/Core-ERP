using System;
using System.Collections.Generic;

namespace Core.Controllers
{
    public static class Auth
    {
        public static Models.User User;
        public static Models.Company Company;

        /// <summary>
        /// Upon initializing this class, we will check the user name and pin code against our local data. 
        /// If no rows found, will check against online records.
        /// </summary>
        /// <param name="User">User's Name</param>
        /// <param name="Pin">User's Security Pin.</param>
        public static void LogIn(string User, string Pin)
        {  
            
        }

        /// <summary>
        /// Function will call the list of companies associated with this user from Cognitivo.in.
        /// </summary>
        /// <returns>List of Companies</returns>
        public static List<Models.Company> Companies()
        {
            return null;
        }
    }
}
