using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
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
                dbclass.GetAllManufacture(ticket, connestionString);
                dbclass.GetAllBrandByManufactureId("", "", "");

                dbclass.GetAllManufacture(ticket, connestionString);
                dbclass.GetAllManufacture("", "");

            }
         //   dbclass.InsertStyleFromFile(connestionString);


        }


    }
}
