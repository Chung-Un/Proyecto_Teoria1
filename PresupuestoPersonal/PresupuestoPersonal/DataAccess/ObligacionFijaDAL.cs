using Microsoft.Data.SqlClient;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.DataAccess
{
    public class ObligacionFijaDAL
    {
        public static void Insertar(ObligacionFija obf, int idUsuario, int creadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_insertar_obligacion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_id_subcategoria", obf.IdSubcategoria);
            cmd.Parameters.AddWithValue("@p_nombre", obf.NombreObligacion);
            cmd.Parameters.AddWithValue("@p_monto", obf.MontoMensual);
            cmd.Parameters.AddWithValue("@p_dia_vencimiento", obf.DiaVencimiento);
            cmd.Parameters.AddWithValue("@p_fecha_inicio", obf.FechaInicio.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@p_fecha_fin", obf.FechaFin.HasValue
                                                               ? obf.FechaFin.Value.ToDateTime(TimeOnly.MinValue)
                                                               : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_creado_por", creadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Actualizar(ObligacionFija obf, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_actualizar_obligacion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_obligacion", obf.IdObligacion);
            cmd.Parameters.AddWithValue("@p_nombre", obf.NombreObligacion);
            cmd.Parameters.AddWithValue("@p_monto", obf.MontoMensual);
            cmd.Parameters.AddWithValue("@p_dia_vencimiento", obf.DiaVencimiento);
            cmd.Parameters.AddWithValue("@p_fecha_fin", obf.FechaFin.HasValue
                                                               ? obf.FechaFin.Value.ToDateTime(TimeOnly.MinValue)
                                                               : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_activo", obf.EstadoVigente);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Eliminar(int idObligacion, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_eliminar_obligacion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_obligacion", idObligacion);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static ObligacionFija Consultar(int idObligacion)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_consultar_obligacion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_obligacion", idObligacion);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ObligacionFija
                {
                    IdObligacion = reader.GetInt32(reader.GetOrdinal("id_obligacion")),
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    NombreObligacion = reader.GetString(reader.GetOrdinal("nombre_obligacion")),
                    MontoMensual = reader.GetDecimal(reader.GetOrdinal("monto_mensual")),
                    DiaVencimiento = reader.GetByte(reader.GetOrdinal("dia_vencimiento")),
                    EstadoVigente = reader.GetBoolean(reader.GetOrdinal("estado_vigente")),
                    FechaInicio = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("fecha_inicio"))),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin"))
                                      ? null
                                      : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("fecha_fin")))
                };
            }
            return null;
        }

        public static List<ObligacionFija> ListarPorUsuario(int idUsuario, bool activo)
        {
            List<ObligacionFija> lista = new List<ObligacionFija>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_obligaciones_usuario", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_activo", activo);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new ObligacionFija
                {
                    IdObligacion = reader.GetInt32(reader.GetOrdinal("id_obligacion")),
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    NombreObligacion = reader.GetString(reader.GetOrdinal("nombre_obligacion")),
                    MontoMensual = reader.GetDecimal(reader.GetOrdinal("monto_mensual")),
                    DiaVencimiento = reader.GetByte(reader.GetOrdinal("dia_vencimiento")),
                    EstadoVigente = reader.GetBoolean(reader.GetOrdinal("estado_vigente")),
                    FechaInicio = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("fecha_inicio"))),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin"))? null: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("fecha_fin")))
                });
            }
            return lista;
        }
    }
}