using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CatalogoApp.Datos
{
    public class DatabaseHelper
    {
        private static string connectionString;
        static DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CatalogoAppDB"].ConnectionString;
        }
    public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
