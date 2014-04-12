using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Our_Calendar.DatabaseHelper
{
    public class DatabaseHelper
    {
        static private MySqlConnection DbConnect()
        {
            // Create database connection APPSETTING_MYSQL_CONNECTION_STRING is from Azure server config 
            // you can replace it with your connection string if deploying to a different server.
            string cs = Environment.GetEnvironmentVariable("APPSETTING_MYSQL_CONNECTION_STRING");
            MySqlConnection Connection = new MySqlConnection(cs);
            return Connection;
        }

        static public Boolean DbDelete(string table, Dictionary<string, string> conditionDictionary)
        {
            MySqlConnection Con = DbConnect();

            try
            {
                Con.Open();
                MySqlCommand ConCmd;
                ConCmd = Con.CreateCommand();

                int i = 1;
                int count = conditionDictionary.Count;
                string conditions = "";
                foreach (KeyValuePair<string, string> entry in conditionDictionary)
                {
                    if (i < count)
                    {
                        conditions += entry.Key + " = @" + entry.Key + " AND ";
                    }
                    else
                    {
                        conditions += entry.Key + " = @" + entry.Key;
                    }
                    i++;
                }

                ConCmd.CommandText = "DELETE FROM " + table + " WHERE " + conditions;

                foreach (KeyValuePair<string, string> entry in conditionDictionary)
                {
                    ConCmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                ConCmd.ExecuteNonQuery();
                Con.Close();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        static public Boolean DbInsert(String table, Dictionary<string, string> values)
        {
            MySqlConnection Con = DbConnect();

            try
            {
                Con.Open();
                MySqlCommand ConCmd;
                ConCmd = Con.CreateCommand();

                int i = 1;
                int count = values.Count;
                string columns = "";
                string parameters = "";
                foreach (KeyValuePair<string, string> entry in values)
                {
                    if (i < count)
                    {
                        columns += entry.Key + ", ";
                        parameters += "@" + entry.Key + ", ";
                    }
                    else
                    {
                        columns += entry.Key;
                        parameters += "@" + entry.Key;
                    }
                    i++;
                }

                ConCmd.CommandText = "INSERT INTO " + table + " (" + columns + ") VALUES (" + parameters + ")";

                foreach (KeyValuePair<string, string> entry in values)
                {
                    ConCmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                ConCmd.ExecuteNonQuery();
                Con.Close();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        static public DataTable DbSelect(string query, Dictionary<string, string> parameters)
        {           
            MySqlConnection Con = DbConnect();
            DataTable dt = new DataTable();

            try
            {                
                Con.Open();
                MySqlCommand ConCmd;
                ConCmd = Con.CreateCommand();

                ConCmd.CommandText = query;

                // Bind select conditions to parameters
                foreach (KeyValuePair<string, string> entry in parameters)
                {
                    ConCmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                MySqlDataAdapter a = new MySqlDataAdapter(ConCmd);
                a.Fill(dt);

                Con.Close();
                return dt;
            }
            catch (Exception err)
            {
                return dt;
            }
        }

        static public Boolean DbUpdate(string table, Dictionary<string, string> conditions, Dictionary<string, string> values)
        {
            // UPDATE table_name SET column1=value1,column2=value2,... WHERE some_column=some_value;
            MySqlConnection Con = DbConnect();

            try
            {
                Con.Open();
                MySqlCommand ConCmd;
                ConCmd = Con.CreateCommand();


                // Create string of values to update (for query below)
                int i = 1;
                int count = values.Count;
                string updateValues = "";
                foreach (KeyValuePair<string, string> entry in values)
                {
                    if (i < count)
                    {
                        updateValues += entry.Key + " = " + "@" + entry.Key + ", ";
                    }
                    else
                    {
                        updateValues += entry.Key + " = " + "@" + entry.Key + ", ";
                    }
                    i++;
                }

                // Create string of WHERE conditions (for query below)
                i = 1;
                count = conditions.Count;
                string updateConditions = "";
                foreach (KeyValuePair<string, string> entry in conditions)
                {
                    if (i < count)
                    {
                        updateConditions += entry.Key + " = " + "@" + entry.Key + ", ";
                    }
                    else
                    {
                        updateConditions += entry.Key + " = " + "@" + entry.Key + ", ";
                    }
                    i++;
                }

                // Put query together.
                ConCmd.CommandText = "UPDATE " + table + " SET " + updateValues + " WHERE " + updateConditions;

                // Bind update values to parameters
                foreach (KeyValuePair<string, string> entry in values)
                {
                    ConCmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                // Bind update conditions to parameters
                foreach (KeyValuePair<string, string> entry in conditions)
                {
                    ConCmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                // Execute the query.                
                if (ConCmd.ExecuteNonQuery() <= 0)
                {
                    // If the number of rows affected by 
                    // the query is 0 or less then return false
                    Con.Close();
                    return false;
                }
                Con.Close();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
    }
}