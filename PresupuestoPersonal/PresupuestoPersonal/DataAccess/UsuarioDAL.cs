using Microsoft.Data.SqlClient;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.DataAccess
{
    public class UsuarioDAL
    {
        public static void Insertar(Usuario user)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_insertar_usuario", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_primer_nombre", user.PrimerNombre);
            cmd.Parameters.AddWithValue("@p_segundo_nombre", user.SegundoNombre);
            cmd.Parameters.AddWithValue("@p_primer_apellido", user.PrimerApellido);
            cmd.Parameters.AddWithValue("@p_segundo_apellido", user.SegundoApellido);
            cmd.Parameters.AddWithValue("@p_email", user.CorreoElectronico);
            cmd.Parameters.AddWithValue("@p_salario_mensual", user.SalarioMensualBase);
            cmd.Parameters.AddWithValue("@p_password", user.Password);
            cmd.Parameters.AddWithValue("@p_creado_por", user.IdUsuario);

            cmd.ExecuteNonQuery();
        }

        public static void Actualizar(Usuario user)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_actualizar_usuario", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", user.IdUsuario);
            cmd.Parameters.AddWithValue("@p_primer_nombre", user.PrimerNombre);
            cmd.Parameters.AddWithValue("@p_segundo_nombre", user.SegundoNombre);
            cmd.Parameters.AddWithValue("@p_primer_apellido", user.PrimerApellido);
            cmd.Parameters.AddWithValue("@p_segundo_apellido", user.SegundoApellido);
            cmd.Parameters.AddWithValue("@p_salario_mensual", user.SalarioMensualBase);

            cmd.ExecuteNonQuery();
        }

        public static void Eliminar(int idUsuario)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_eliminar_usuario", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);

            cmd.ExecuteNonQuery();
        }

        public static Usuario Consultar(int idUsuario)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_consultar_usuario", conexion);  
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    CorreoElectronico = reader.GetString(reader.GetOrdinal("correo_electronico")),
                    PrimerNombre = reader.GetString(reader.GetOrdinal("primer_nombre")),
                    SegundoNombre = reader.GetString(reader.GetOrdinal("segundo_nombre")),
                    PrimerApellido = reader.GetString(reader.GetOrdinal("primer_apellido")),
                    SegundoApellido = reader.GetString(reader.GetOrdinal("segundo_apellido")),
                    FechaIngreso = reader.GetDateTime(reader.GetOrdinal("fecha_ingreso")),
                    SalarioMensualBase = reader.GetDecimal(reader.GetOrdinal("salario_mensual_base")),
                    EstadoUsuario = reader.GetBoolean(reader.GetOrdinal("estado_usuario"))
                };
            }
            return null;
        }

        public static List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_listar_usuarios", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                    CorreoElectronico = reader.GetString(reader.GetOrdinal("correo_electronico")),
                    PrimerNombre = reader.GetString(reader.GetOrdinal("nombre_completo")),
                    EstadoUsuario = reader.GetString(reader.GetOrdinal("estado")) == "Activo"
                });
            }
            return lista;
        }

        public static Usuario Login(string correo, string password)
        {
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_login_usuario", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_correo", correo);
            cmd.Parameters.AddWithValue("@p_password", password);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    CorreoElectronico = reader.GetString(reader.GetOrdinal("correo_electronico")),
                    PrimerNombre = reader.GetString(reader.GetOrdinal("primer_nombre")),
                    SegundoNombre = reader.GetString(reader.GetOrdinal("segundo_nombre")),
                    PrimerApellido = reader.GetString(reader.GetOrdinal("primer_apellido")),
                    SegundoApellido = reader.GetString(reader.GetOrdinal("segundo_apellido")),
                    FechaIngreso = reader.GetDateTime(reader.GetOrdinal("fecha_ingreso")),
                    SalarioMensualBase = reader.GetDecimal(reader.GetOrdinal("salario_mensual_base")),
                    EstadoUsuario = reader.GetBoolean(reader.GetOrdinal("estado_usuario"))
                };
            }
            return null;
        }
    }
}