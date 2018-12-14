using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Controllers
{
    public class Session
    {
        private Context _db;
        public Session(Context db)
        {
            _db = db;
        }
        public Core.Models.Session OpenSession()
        {
            Core.Models.Session session = new Core.Models.Session();
            session.name = "Sales";
            session.startDate = DateTime.Now;
            session.startingBalance = 0;
            return session;

        }
    }
}
