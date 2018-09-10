using System;
using System.Collections.Generic;

namespace Core.ViewModels.Commercial
{
    public class Sales
    {
        List<Models.Orders.Order> Orders { get; set; }

        public void List()
        {
            return;
        }

        public void Search()
        {
            return;
        }

        public Boolean Save(Models.Orders.Order Order)
        {
            return true;
        }

        public Boolean Approve(Models.Orders.Order Order)
        {
            return true;
        }

        public Boolean Annull(Models.Orders.Order Order)
        {
            return true;
        }

        public Boolean Delete(Models.Orders.Order Order)
        {
            return true;
        }
    }
}
