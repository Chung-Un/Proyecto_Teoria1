using Microsoft.Data.SqlClient;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.DataAccess
{
    public class CategoriaDAL
    {
        public static void Insertar(Categoria cat)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_insertar_categoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_nombre_categoria", cat.NombreCategoria);
            cmd.Parameters.AddWithValue("@p_tipo_categoria", cat.TipoCategoria);
            cmd.Parameters.AddWithValue("@p_id_usuario", 1);
            cmd.Parameters.AddWithValue("@p_creado_por", 1);

            cmd.ExecuteNonQuery();
        }

        public static void Actualizar(Categoria cat, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_actualizar_categoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_categoria", cat.IdCategoria);
            cmd.Parameters.AddWithValue("@p_nombre_categoria", cat.NombreCategoria);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static void Eliminar(int idCategoria, int modificadoPor)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_eliminar_categoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_categoria", idCategoria);
            cmd.Parameters.AddWithValue("@p_modificado_por", modificadoPor);

            cmd.ExecuteNonQuery();
        }

        public static Categoria Consultar(int idCategoria)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_consultar_categoria", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_categoria", idCategoria);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Categoria
                {
                    IdCategoria = reader.GetInt32(reader.GetOrdinal("id_categoria")),
                    NombreCategoria = reader.GetString(reader.GetOrdinal("nombre_categoria")),
                    TipoCategoria = reader.GetString(reader.GetOrdinal("tipo_categoria")),
                    NombreIcono = reader.IsDBNull(reader.GetOrdinal("nombre_icono"))? null: reader.GetString(reader.GetOrdinal("nombre_icono")),
                    ColorHexadecimal = reader.IsDBNull(reader.GetOrdinal("color_hexademical"))? null: reader.GetString(reader.GetOrdinal("color_hexademical")),
                    OrdenPresentacion = reader.IsDBNull(reader.GetOrdinal("orden_presentacion")) ? 0: reader.GetInt32(reader.GetOrdinal("orden_presentacion"))
                };
            }
            return null;
        }

        public static List<Categoria> Listar(int idUsuario, string tipo)
        {
            List<Categoria> lista = new List<Categoria>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_categorias", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_tipo", tipo ?? (object)DBNull.Value);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Categoria
                {
                    IdCategoria = reader.GetInt32(reader.GetOrdinal("id_categoria")),
                    NombreCategoria = reader.GetString(reader.GetOrdinal("nombre_categoria")),
                    TipoCategoria = reader.GetString(reader.GetOrdinal("tipo_categoria")),
                    NombreIcono = reader.IsDBNull(reader.GetOrdinal("nombre_icono"))? null: reader.GetString(reader.GetOrdinal("nombre_icono")),
                    ColorHexadecimal = reader.IsDBNull(reader.GetOrdinal("color_hexademical"))? null: reader.GetString(reader.GetOrdinal("color_hexademical")),
                    OrdenPresentacion = reader.IsDBNull(reader.GetOrdinal("orden_presentacion"))? 0: reader.GetInt32(reader.GetOrdinal("orden_presentacion"))
                });
            }
            return lista;
        }
    }
}