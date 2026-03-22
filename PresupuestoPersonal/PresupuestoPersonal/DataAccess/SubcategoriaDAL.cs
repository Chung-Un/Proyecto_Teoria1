using Microsoft.Data.SqlClient;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.DataAccess
{
    public class SubcategoriaDAL
    {
        public static void Insertar(Subcategoria sub, int creadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_insertar_subcategoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_subcategoria", sub.IdSubcategoria);
            cmd.Parameters.AddWithValue("@p_nombre_subcategoria", sub.NombreSubcategoria);
            cmd.Parameters.AddWithValue("@p_descripcion", DBNull.Value);
            cmd.Parameters.AddWithValue("@p_es_defecto", sub.SubcategoriaPorDefecto);
            cmd.Parameters.AddWithValue("@p_creado_por", creadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Actualizar(Subcategoria sub, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_actualizar_subcategoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_subcategoria", sub.IdSubcategoria);
            cmd.Parameters.AddWithValue("@p_nombre_subcategoria", sub.NombreSubcategoria);
            cmd.Parameters.AddWithValue("@p_descripcion", DBNull.Value);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Eliminar(int idSubcategoria, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_eliminar_subcategoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_subcategoria", idSubcategoria);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static Subcategoria Consultar(int idSubcategoria)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_consultar_subcategoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_subcategoria", idSubcategoria);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Subcategoria
                {
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    IdCategoria = reader.GetInt32(reader.GetOrdinal("id_categoria")),
                    NombreSubcategoria = reader.GetString(reader.GetOrdinal("nombre_subcategoria")),
                    EstadoSubcategoria = reader.GetBoolean(reader.GetOrdinal("estado_subcategoria")),
                    SubcategoriaPorDefecto = reader.GetBoolean(reader.GetOrdinal("subcategoria_por_defecto"))
                };
            }
            return null;
        }

        public static List<Subcategoria> ListarPorCategoria(int idCategoria)
        {
            List<Subcategoria> lista = new List<Subcategoria>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_subcategorias_por_categoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_categoria", idCategoria);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Subcategoria
                {
                    IdSubcategoria = reader.GetInt32(reader.GetOrdinal("id_subcategoria")),
                    IdCategoria = reader.GetInt32(reader.GetOrdinal("id_categoria")),
                    NombreSubcategoria = reader.GetString(reader.GetOrdinal("nombre_subcategoria")),
                    EstadoSubcategoria = reader.GetBoolean(reader.GetOrdinal("estado_subcategoria")),
                    SubcategoriaPorDefecto = reader.GetBoolean(reader.GetOrdinal("subcategoria_por_defecto"))
                });
            }
            return lista;
        }
    }
}