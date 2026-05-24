using System.Configuration;
using System.Data.SqlClient;

namespace CatalogoApp.Datos
{
    public static class DatabaseHelper
    {
        private static string connectionString;

        static DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CatalogoDB"].ConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
