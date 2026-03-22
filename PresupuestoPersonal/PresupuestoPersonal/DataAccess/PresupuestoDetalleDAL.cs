using Microsoft.Data.SqlClient;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.DataAccess
{
    public class PresupuestoDetalleDAL
    {
        public static void Insertar(PresupuestoDetalle pd, int creadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_insertar_presupuesto_detalle", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_presupuesto", pd.IdPresupuesto);
            cmd.Parameters.AddWithValue("@p_id_subcategoria", pd.IdSubcategoria);
            cmd.Parameters.AddWithValue("@p_monto_mensual", pd.MontoMensual);
            cmd.Parameters.AddWithValue("@p_observaciones", pd.Observaciones ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_creado_por", creadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Actualizar(PresupuestoDetalle pd, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_actualizar_presupuesto_detalle", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_detalle", pd.IdDetalle);
            cmd.Parameters.AddWithValue("@p_monto_mensual", pd.MontoMensual);
            cmd.Parameters.AddWithValue("@p_observaciones", pd.Observaciones ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Eliminar(int idDetalle, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_eliminar_presupuesto_detalle", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_detalle", idDetalle);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static PresupuestoDetalle Consultar(int idDetalle)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_consultar_presupuesto_detalle", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_detalle", idDetalle);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PresupuestoDetalle
                {
                    IdDetalle = reader.GetInt32(reader.GetOrdinal("id_detalle")),
                    IdPresupuesto = reader.GetInt32(reader.GetOrdinal("id_presupuesto")),
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    MontoMensual = reader.GetDecimal(reader.GetOrdinal("monto_mensual")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones"))
                                     ? null
                                     : reader.GetString(reader.GetOrdinal("observaciones"))
                };
            }
            return null;
        }

        public static List<PresupuestoDetalle> ListarPorPresupuesto(int idPresupuesto)
        {
            List<PresupuestoDetalle> lista = new List<PresupuestoDetalle>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_detalles_presupuesto", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new PresupuestoDetalle
                {
                    IdDetalle = reader.GetInt32(reader.GetOrdinal("id_detalle")),
                    IdPresupuesto = reader.GetInt32(reader.GetOrdinal("id_presupuesto")),
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    MontoMensual = reader.GetDecimal(reader.GetOrdinal("monto_mensual")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones"))
                                     ? null
                                     : reader.GetString(reader.GetOrdinal("observaciones"))
                });
            }
            return lista;
        }
    }
}