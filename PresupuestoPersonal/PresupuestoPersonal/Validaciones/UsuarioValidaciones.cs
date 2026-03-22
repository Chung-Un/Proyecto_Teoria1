using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Validaciones
{
    public class UsuarioValidaciones
    {
        public static void Insertar(Usuario user)
        {
            if (string.IsNullOrEmpty(user.PrimerNombre))
                throw new Exception("Primer nombre es obligatorio");
            if (string.IsNullOrEmpty(user.PrimerApellido))
                throw new Exception("Primer apellido es obligatorio.");
            if (string.IsNullOrEmpty(user.CorreoElectronico))
                throw new Exception("Correo electronico es obligatorio.");
            if (string.IsNullOrEmpty(user.Password))
                throw new Exception("Contrasena es obligatoria.");
            if (user.SalarioMensualBase <= 0)
                throw new Exception("Salario debe ser mayor a cero.");

            UsuarioDAL.Insertar(user);
        }

        public static void Actualizar(Usuario user)
        {
            if (user.IdUsuario <= 0)
                throw new Exception("ID de usuario no valido");
            if (string.IsNullOrEmpty(user.PrimerNombre))
                throw new Exception("Primer nombre es obligatorio");
            if (string.IsNullOrEmpty(user.PrimerApellido))
                throw new Exception("Primer apellido es obligatorio.");
            if (user.SalarioMensualBase <= 0)
                throw new Exception("Salario debe ser mayor a cero.");

            UsuarioDAL.Actualizar(user);
        }

        public static void Eliminar(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new Exception("ID de usuario no valido");

            UsuarioDAL.Eliminar(idUsuario);
        }

        public static Usuario Leer(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new Exception("ID de usuario no valido");

            Usuario user = UsuarioDAL.Consultar(idUsuario);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            return user;
        }

        public static List<Usuario> Listar()
        {
            return UsuarioDAL.Listar();
        }

        public static Usuario Login(string correo, string password)
        {
            if (string.IsNullOrEmpty(correo))
                throw new Exception("Correo es obligatorio.");
            if (string.IsNullOrEmpty(password))
                throw new Exception("Contrasena es obligatorio");

            List<Usuario> usuarios = UsuarioDAL.Listar();
            Usuario user = usuarios.FirstOrDefault(u =>
                u.CorreoElectronico == correo &&
                u.Password == password);

            if (user == null)
                throw new Exception("Correo o contrasena incorrecto.");
            if (!user.EstadoUsuario)
                throw new Exception("Usuario inactivo.");

            return user;
        }
    }
}