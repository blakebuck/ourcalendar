using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.EntitySql;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Our_Calendar.Models
{
    public class HomeModels
    {

    }

    public class CallDatabase
    {
        public static string ReturnName()
        {
            MySqlConnection connection = new MySqlConnection(Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING"));
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT fullName FROM Users LIMIT 1";
                MySqlDataAdapter a = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                a.Fill(dt);
                return dt.Rows.Count >= 1 ? Convert.ToString(dt.Rows[0][0]) : "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}