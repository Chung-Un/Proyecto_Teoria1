using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Validaciones
{
    public class CategoriaValidaciones
    {
        public static void Insertar(Categoria cat)
        {
            if (string.IsNullOrEmpty(cat.NombreCategoria))
                throw new Exception("Nombre de categoria es obligatorio.");
            if (string.IsNullOrEmpty(cat.TipoCategoria))
                throw new Exception("Tipo de categoria es obligatorio.");
            if (cat.TipoCategoria != "ingreso" && cat.TipoCategoria != "gasto" && cat.TipoCategoria != "ahorro")
                throw new Exception("Tipo de categoria debe ser ingreso, gasto o ahorro.");

            CategoriaDAL.Insertar(cat);
        }

        public static void Actualizar(Categoria cat)
        {
            if (cat.IdCategoria <= 0)
                throw new Exception("ID de categoria no valido.");
            if (string.IsNullOrEmpty(cat.NombreCategoria))
                throw new Exception("Nombre de categoria es obligatorio.");

            CategoriaDAL.Actualizar(cat);
        }

        public static void Eliminar(int idCategoria, int modificadoPor)
        {
            if (idCategoria <= 0)
                throw new Exception("ID de categoria no valido.");

            CategoriaDAL.Eliminar(idCategoria, modificadoPor);
        }

        public static Categoria Leer(int idCategoria)
        {
            if (idCategoria <= 0)
                throw new Exception("ID de categoria no valido.");

            Categoria cat = CategoriaDAL.Consultar(idCategoria);
            if (cat == null)
                throw new Exception("Categoria no encontrada.");

            return cat;
        }

        public static List<Categoria> Listar(int idUsuario, string tipo)
        {
            return CategoriaDAL.Listar(idUsuario, tipo);
        }
    }
}