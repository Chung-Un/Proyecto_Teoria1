using Microsoft.Data.SqlClient;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.DataAccess
{
    public class TransaccionDAL
    {
        public static void Insertar(Transaccion t)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_insertar_transaccion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", t.IdUsuario);
            cmd.Parameters.AddWithValue("@p_id_presupuesto", DBNull.Value);
            cmd.Parameters.AddWithValue("@p_id_detalle", t.IdDetalle);
            cmd.Parameters.AddWithValue("@p_anio_transaccion", t.AnioTransaccion);
            cmd.Parameters.AddWithValue("@p_mes_transaccion", t.MesTransaccion);
            cmd.Parameters.AddWithValue("@p_id_subcategoria", t.IdSubcategoria);
            cmd.Parameters.AddWithValue("@p_id_obligacion", DBNull.Value);
            cmd.Parameters.AddWithValue("@p_tipo_transaccion", t.TipoTransaccion);
            cmd.Parameters.AddWithValue("@p_descripcion_movimiento", t.DescripcionMovimiento);
            cmd.Parameters.AddWithValue("@p_monto_transaccion", t.MontoTransaccion);
            cmd.Parameters.AddWithValue("@p_fecha_transaccion", t.FechaTransaccion.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@p_metodo_pago", t.MetodoPago);
            cmd.Parameters.AddWithValue("@p_numero_factura", t.NumeroFactura.HasValue
                                                                       ? t.NumeroFactura.Value
                                                                       : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_observaciones", t.Observaciones ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_creado_por", t.IdUsuario);

            cmd.ExecuteNonQuery();
        }

        public static void Actualizar(Transaccion t, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_actualizar_transaccion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_transaccion", t.IdTransaccion);
            cmd.Parameters.AddWithValue("@p_anio_transaccion", t.AnioTransaccion);
            cmd.Parameters.AddWithValue("@p_mes_transaccion", t.MesTransaccion);
            cmd.Parameters.AddWithValue("@p_descripcion_movimiento", t.DescripcionMovimiento);
            cmd.Parameters.AddWithValue("@p_monto_transaccion", t.MontoTransaccion);
            cmd.Parameters.AddWithValue("@p_fecha_transaccion", t.FechaTransaccion.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@p_metodo_pago", t.MetodoPago);
            cmd.Parameters.AddWithValue("@p_numero_factura", t.NumeroFactura.HasValue
                                                                       ? t.NumeroFactura.Value
                                                                       : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_observaciones", t.Observaciones ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Eliminar(int idTransaccion, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_eliminar_transaccion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_transaccion", idTransaccion);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static Transaccion Consultar(int idTransaccion)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_consultar_transaccion", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_transaccion", idTransaccion);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Transaccion
                {
                    IdTransaccion = reader.GetInt32(reader.GetOrdinal("id_transaccion")),
                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                    IdDetalle = reader.GetInt32(reader.GetOrdinal("id_detalle")),
                    AnioTransaccion = reader.GetInt16(reader.GetOrdinal("anio_transaccion")),
                    MesTransaccion = reader.GetByte(reader.GetOrdinal("mes_transaccion")),
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    TipoTransaccion = reader.GetString(reader.GetOrdinal("tipo_transaccion")),
                    DescripcionMovimiento = reader.GetString(reader.GetOrdinal("descripcion_movimiento")),
                    MontoTransaccion = reader.GetDecimal(reader.GetOrdinal("monto_transaccion")),
                    FechaTransaccion = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("fecha_transaccion"))),
                    MetodoPago = reader.GetString(reader.GetOrdinal("metodo_pago")),
                    NumeroFactura = reader.IsDBNull(reader.GetOrdinal("numero_factura"))
                                           ? null
                                           : reader.GetInt32(reader.GetOrdinal("numero_factura")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones"))
                                           ? null
                                           : reader.GetString(reader.GetOrdinal("observaciones")),
                    FechaYHoraRegistro = reader.GetDateTime(reader.GetOrdinal("fecha_y_hora_registro"))
                };
            }
            return null;
        }

        public static List<Transaccion> ListarPorPresupuesto(int idPresupuesto, short anio, byte mes, string tipo)
        {
            List<Transaccion> lista = new List<Transaccion>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_transacciones_presupuestos", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);
            cmd.Parameters.AddWithValue("@p_anio_transaccion", anio);
            cmd.Parameters.AddWithValue("@p_mes", mes);
            cmd.Parameters.AddWithValue("@p_tipo_transaccion", tipo ?? (object)DBNull.Value);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Transaccion
                {
                    IdTransaccion = reader.GetInt32(reader.GetOrdinal("id_transaccion")),
                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                    IdDetalle = reader.GetInt32(reader.GetOrdinal("id_detalle")),
                    AnioTransaccion = reader.GetInt16(reader.GetOrdinal("anio_transaccion")),
                    MesTransaccion = reader.GetByte(reader.GetOrdinal("mes_transaccion")),
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    TipoTransaccion = reader.GetString(reader.GetOrdinal("tipo_transaccion")),
                    DescripcionMovimiento = reader.GetString(reader.GetOrdinal("descripcion_movimiento")),
                    MontoTransaccion = reader.GetDecimal(reader.GetOrdinal("monto_transaccion")),
                    FechaTransaccion = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("fecha_transaccion"))),
                    MetodoPago = reader.GetString(reader.GetOrdinal("metodo_pago")),
                    NumeroFactura = reader.IsDBNull(reader.GetOrdinal("numero_factura"))
                                            ? null
                                            : reader.GetInt32(reader.GetOrdinal("numero_factura")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones"))
                                            ? null
                                            : reader.GetString(reader.GetOrdinal("observaciones")),
                    FechaYHoraRegistro = reader.GetDateTime(reader.GetOrdinal("fecha_y_hora_registro"))
                });
            }
            return lista;
        }

        public static void RegistrarCompleta(Transaccion t, int creadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_registrar_transaccion_completa", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", t.IdUsuario);
            cmd.Parameters.AddWithValue("@p_id_presupuesto", t.IdDetalle);
            cmd.Parameters.AddWithValue("@p_anio", t.AnioTransaccion);
            cmd.Parameters.AddWithValue("@p_mes", t.MesTransaccion);
            cmd.Parameters.AddWithValue("@p_id_subcategoria", t.IdSubcategoria);
            cmd.Parameters.AddWithValue("@p_tipo_transaccion", t.TipoTransaccion);
            cmd.Parameters.AddWithValue("@p_descripcion_movimiento", t.DescripcionMovimiento);
            cmd.Parameters.AddWithValue("@p_monto_transaccion", t.MontoTransaccion);
            cmd.Parameters.AddWithValue("@p_fecha_transaccion", t.FechaTransaccion.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@p_metodo_pago", t.MetodoPago);
            cmd.Parameters.AddWithValue("@p_creado_por", creadoPor);

            cmd.ExecuteNonQuery();
        }
    }
}