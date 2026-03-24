using Microsoft.Data.SqlClient;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.DataAccess
{
    public class PresupuestoDAL
    {
        public static void Insertar(Presupuesto p, string descripcion)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_insertar_presupuesto", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", p.IdUsuario);
            cmd.Parameters.AddWithValue("@p_nombre_presupuesto", p.NombrePresupuesto);
            cmd.Parameters.AddWithValue("@p_anio_inicio", p.AnioInicio);
            cmd.Parameters.AddWithValue("@p_mes_inicio", p.MesInicio);
            cmd.Parameters.AddWithValue("@p_mes_fin", p.MesFin);
            cmd.Parameters.AddWithValue("@p_anio_fin", p.AnioFin);
            cmd.Parameters.AddWithValue("@p_descripcion_presupuesto", descripcion);

            cmd.ExecuteNonQuery();
        }

        public static void Actualizar(Presupuesto p, string descripcion, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_actualizar_presupuesto", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_presupuesto", p.IdPresupuesto);
            cmd.Parameters.AddWithValue("@p_nombre_presupuesto", p.NombrePresupuesto);
            cmd.Parameters.AddWithValue("@p_descripcion_presupuesto", descripcion);
            cmd.Parameters.AddWithValue("@p_anio_inicio", p.AnioInicio);
            cmd.Parameters.AddWithValue("@p_mes_inicio", p.MesInicio);
            cmd.Parameters.AddWithValue("@p_mes_fin", p.MesFin);
            cmd.Parameters.AddWithValue("@p_anio_fin", p.AnioFin);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Eliminar(int idPresupuesto, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_eliminar_presupuesto", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static Presupuesto Consultar(int idPresupuesto)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_consultar_presupuesto", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Presupuesto
                {
                    IdPresupuesto = reader.GetInt32(reader.GetOrdinal("id_presupuesto")),
                    NombrePresupuesto = reader.GetString(reader.GetOrdinal("nombre_presupuesto")),
                    AnioInicio = reader.GetInt16(reader.GetOrdinal("anio_inicio")),
                    MesInicio = reader.GetByte(reader.GetOrdinal("mes_inicio")),
                    AnioFin = reader.GetInt16(reader.GetOrdinal("anio_fin")),
                    MesFin = reader.GetByte(reader.GetOrdinal("mes_fin")),
                    TotalIngresosPlanificados = reader.GetDecimal(reader.GetOrdinal("total_ingresos_planificados")),
                    TotalGastosPlanificados = reader.GetDecimal(reader.GetOrdinal("total_gastos_planificados")),
                    TotalAhorroPlanificado = reader.GetDecimal(reader.GetOrdinal("total_ahorro_planificado")),
                    FechaYHoraCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_y_hora_creacion")),
                    EstadoPresupuesto = reader.GetString(reader.GetOrdinal("estado_presupuesto"))
                };
            }
            return null;
        }

        public static List<Presupuesto> ListarPorUsuario(int idUsuario, string estado)
        {
            List<Presupuesto> lista = new List<Presupuesto>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_presupuestos_usuario", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_estado", string.IsNullOrEmpty(estado)
                                                          ? (object)DBNull.Value
                                                          : estado); 
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Presupuesto
                {
                    IdPresupuesto = reader.GetInt32(reader.GetOrdinal("id_presupuesto")),
                    NombrePresupuesto = reader.GetString(reader.GetOrdinal("nombre_presupuesto")),
                    AnioInicio = reader.GetInt16(reader.GetOrdinal("anio_inicio")),
                    MesInicio = reader.GetByte(reader.GetOrdinal("mes_inicio")),
                    AnioFin = reader.GetInt16(reader.GetOrdinal("anio_fin")),
                    MesFin = reader.GetByte(reader.GetOrdinal("mes_fin")),
                    TotalIngresosPlanificados = reader.GetDecimal(reader.GetOrdinal("total_ingresos_planificados")),
                    TotalGastosPlanificados = reader.GetDecimal(reader.GetOrdinal("total_gastos_planificados")),
                    TotalAhorroPlanificado = reader.GetDecimal(reader.GetOrdinal("total_ahorro_planificado")),
                    EstadoPresupuesto = reader.GetString(reader.GetOrdinal("estado_presupuesto"))
                });
            }
            return lista;
        }

        public static List<Presupuesto> ListarTodos(string estado)
        {
            List<Presupuesto> lista = new List<Presupuesto>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_todos_presupuestos", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_estado", string.IsNullOrEmpty(estado)
                                                      ? (object)DBNull.Value
                                                      : estado);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Presupuesto
                {
                    IdPresupuesto = reader.GetInt32(reader.GetOrdinal("id_presupuesto")),
                    NombrePresupuesto = reader.GetString(reader.GetOrdinal("nombre_presupuesto")),
                    AnioInicio = reader.GetInt16(reader.GetOrdinal("anio_inicio")),
                    MesInicio = reader.GetByte(reader.GetOrdinal("mes_inicio")),
                    AnioFin = reader.GetInt16(reader.GetOrdinal("anio_fin")),
                    MesFin = reader.GetByte(reader.GetOrdinal("mes_fin")),
                    TotalIngresosPlanificados = reader.GetDecimal(reader.GetOrdinal("total_ingresos_planificados")),
                    TotalGastosPlanificados = reader.GetDecimal(reader.GetOrdinal("total_gastos_planificados")),
                    TotalAhorroPlanificado = reader.GetDecimal(reader.GetOrdinal("total_ahorro_planificado")),
                    EstadoPresupuesto = reader.GetString(reader.GetOrdinal("estado_presupuesto"))
                });
            }
            return lista;
        }

        public static void CrearCompleto(Presupuesto p, string descripcion, string listaSubcategoriasJson, int creadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_crear_presupuesto_completo", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", p.IdUsuario);
            cmd.Parameters.AddWithValue("@p_nombre_presupuesto", p.NombrePresupuesto);
            cmd.Parameters.AddWithValue("@p_descripcion_presupuesto", descripcion);
            cmd.Parameters.AddWithValue("@p_anio_inicio", p.AnioInicio);
            cmd.Parameters.AddWithValue("@p_mes_inicio", p.MesInicio);
            cmd.Parameters.AddWithValue("@p_anio_fin", p.AnioFin);
            cmd.Parameters.AddWithValue("@p_mes_fin", p.MesFin);
            cmd.Parameters.AddWithValue("@p_lista_subcategorias_json", listaSubcategoriasJson);
            cmd.Parameters.AddWithValue("@p_creado_por", creadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Cerrar(int idPresupuesto, int modificadoPor,
                                  out decimal totalIngresos, out decimal totalGastos, out decimal totalAhorros)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_cerrar_presupuesto", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            
            SqlParameter pIngresos = new SqlParameter("@p_total_ingresos", System.Data.SqlDbType.Decimal);
            SqlParameter pGastos = new SqlParameter("@p_total_gastos", System.Data.SqlDbType.Decimal);
            SqlParameter pAhorros = new SqlParameter("@p_total_ahorros", System.Data.SqlDbType.Decimal);
            pIngresos.Direction = System.Data.ParameterDirection.Output;
            pGastos.Direction = System.Data.ParameterDirection.Output;
            pAhorros.Direction = System.Data.ParameterDirection.Output;

            cmd.Parameters.Add(pIngresos);
            cmd.Parameters.Add(pGastos);
            cmd.Parameters.Add(pAhorros);

            cmd.ExecuteNonQuery();

            totalIngresos = (decimal)pIngresos.Value;
            totalGastos = (decimal)pGastos.Value;
            totalAhorros = (decimal)pAhorros.Value;
        }
    }
}