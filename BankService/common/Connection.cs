using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace BankService.common
{
    public class Connection
    {
        public static string PATHextr;
        public static string SqlCon;
        public static SqlConnection con;
        public static SqlCommand cmd;
        public static SqlDataReader datar;
        public static SqlDataAdapter Adapter;

        public static void openconnection()
        {
            string SettingsPath = HttpContext.Current.Server.MapPath("\\dbOptions.xml");
            XmlDocument settings = new XmlDocument();
            settings.Load(SettingsPath);
            // Select a specific node
            XmlNode node = settings.SelectSingleNode("/Settings/ConnectionString/Value");
            // Get its value
            SqlCon = node.InnerText;
            con = new SqlConnection(SqlCon);
            con.Open();
        }
    }
}