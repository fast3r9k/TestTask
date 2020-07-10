using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using System.Data.SqlClient;
using HelperClass;
using System.Xml;

namespace CbParse1
{
    class Program
    {
        static string connectionString = @"Data source = KYLAK-ПК;" + "Integrated security = true; Initial Catalog = ValuteTable";
        SqlConnectionStringBuilder connectionStringBuilder =
            new SqlConnectionStringBuilder(connectionString);
        static void Main(string[] args)
        {
             Console.WriteLine($"{DateTime.Now} start");
             PushDataClass.FeelBigTable();            
             PushDataClass.InitDictionary();
             Console.WriteLine(PushDataClass.GetRate("EUR", new DateTime(2020,5,5)));
             Console.WriteLine($"{DateTime.Now} end");
        }

    }
}