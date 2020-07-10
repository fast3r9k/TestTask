using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HelperClass
{
    public static class PushDataClass
    {
        public static void InitDictionary()
        {
            string uri = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=";
            WebClient client = new WebClient();
            Encoding win1251 = Encoding.GetEncoding("windows-1251");
            UTF8Encoding utf = new UTF8Encoding();
            Byte[] encodedBytes = win1251.GetBytes(uri);
            uri = utf.GetString(encodedBytes);
            var xml = client.DownloadString(uri);
            XDocument xdoc = XDocument.Parse(xml);
            var el = xdoc.Element("ValCurs").Elements("Valute");
            if (!object.ReferenceEquals(el, null) && el.Count() > 0)
            {
                using (SqlConnection connection = new SqlConnection(@"Data source = KYLAK-ПК;" + "Integrated security = true; Initial Catalog = ValuteTable"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    try
                    {
                        foreach (XElement Valute in el)
                        {

                            command.CommandText =
                            $@"
                            INSERT INTO dbo.Valute (ValuteId, ValuteName, CharCode)
                                VALUES(
		                        NEWID(),
		                        '{Valute.Element("Name").Value}',
		                        '{Valute.Element("CharCode").Value}'
	                                  )
                            ";
                            command.ExecuteNonQuery();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.InnerException?.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public static void FeelBigTable()
        {
            string uri = string.Empty;
            DateTime date = DateTime.Now;
            using (SqlConnection connection = new SqlConnection(@"Data source = KYLAK-ПК;" + "Integrated security = true; Initial Catalog = ValuteTable"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                while (date <= DateTime.Now)
                {
                    uri = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=";
                    uri += date.ToShortDateString();
                    WebRequest req = WebRequest.Create(uri);
                    req.Timeout = int.MaxValue;
                    WebResponse response = req.GetResponse();
                    string xml = string.Empty;
                    using (Stream reader = response.GetResponseStream())
                    {
                        using (StreamReader readerr = new StreamReader(reader))
                        {
                            xml = readerr.ReadToEnd();
                        }
                    }
                    XDocument xdoc = XDocument.Parse(xml);
                    var el = xdoc.Element("ValCurs").Elements("Valute");
                    foreach (XElement Valute in el)
                    {
                        command.CommandText =
                        $@"
                           INSERT INTO dbo.ValuteTable1 (Nominal, Rate, CharCode, RateDate)
                               VALUES(
		                              {Valute.Element("Nominal").Value},
		                               {Valute.Element("Value").Value.Replace(",", ".")},
		                               '{Valute.Element("CharCode").Value}',
		                               '{date}'
	                                 )
                           ";
                        command.ExecuteNonQuery();
                    }
                    date = date.AddDays(1);
                }
                connection.Close();
            }
        }
        public static float GetRate(string charCode, DateTime date)
        {
            using (SqlConnection connection = new SqlConnection(@"Data source = KYLAK-ПК;" + "Integrated security = true; Initial Catalog = ValuteTable"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $@"SELECT * FROM dbo.GetValute('{charCode}','{date}')";
                float Rate = float.Parse(command.ExecuteScalar().ToString());
                return Rate;
            }
        }
    }
}
