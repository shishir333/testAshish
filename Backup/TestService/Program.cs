using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace TestService
{
    class Program
    {
        static void Main(string[] args)
        {
            DBClass dbclass = new DBClass();
            string connestionString = ConfigurationSettings.AppSettings["ConnStr"];
            string ticket = dbclass.GetAuthorizationTicket();
            if (!string.IsNullOrEmpty(ticket))
            {
                dbclass.GetAllManufacture(ticket,connestionString);
            }
        }
    }
}
