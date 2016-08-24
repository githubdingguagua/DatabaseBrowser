using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DbBrowser
{
    //Creating global Variables to save the connection string and check if the application is connected to a database
    public static class Settings
    {
        public static string ConnectionString { get; set; }
        public static bool Connected = false;
    }
}
