using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DbBrowser
{
    public class SqlHelper
    {
        //This application is not protected against SQL Injections, since its a small school project and not used in a real world envirement, where people could take advantage of the vulnerabilities.
        //Get a list of databases
        public static IEnumerable<string> GetDatabases()
        {
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT name FROM Sys.Databases ORDER BY name", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return reader.GetValue(0) as string;
                }
            }
        }

        //Get a list of tables per database
        public static IEnumerable<string> GetTablesForDatabase(string database)
        {
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                connection.Open();
                string query = String.Format("SELECT name FROM {0}.Sys.Tables ORDER BY name", database);
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return reader.GetValue(0) as string;
                }
            }
        }

        //Execute a query on the current database
        public static DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                DataTable datatable = new DataTable();

                datatable.Load(command.ExecuteReader());

                return datatable;
            }
        }
    }
}
