using Microsoft.Data.SqlClient;

namespace PresupuestoPersonal.DataAccess
{
    public class Conexion
    {
        private static string connectionString =
            "Server=localhost\\SQLEXPRESS;Database=PresupuestoPersonal;Integrates Security=True;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection(connectionString);
            conexion.Open();
            return conexion;
        }
    }

}
