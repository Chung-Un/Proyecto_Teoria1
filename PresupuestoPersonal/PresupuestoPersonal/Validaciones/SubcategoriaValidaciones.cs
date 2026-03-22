using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Validaciones
{
    public class SubcategoriaValidaciones
    {
        public static void Insertar(Subcategoria sub, int creadoPor)
        {
            if (sub.IdCategoria <= 0)
                throw new Exception("ID de categoria no valido.");
            if (string.IsNullOrEmpty(sub.NombreSubcategoria))
                throw new Exception("Nombre de subcategoria es obligatorio.");

            SubcategoriaDAL.Insertar(sub, creadoPor);
        }

        public static void Actualizar(Subcategoria sub, int modificadoPor)
        {
            if (sub.IdSubcategoria <= 0)
                throw new Exception("ID de subcategoria no valido.");
            if (string.IsNullOrEmpty(sub.NombreSubcategoria))
                throw new Exception("Nombre de subcategoria es obligatorio.");

            SubcategoriaDAL.Actualizar(sub, modificadoPor);
        }

        public static void Eliminar(int idSubcategoria, int modificadoPor)
        {
            if (idSubcategoria <= 0)
                throw new Exception("ID de subcategoria no valido.");

            SubcategoriaDAL.Eliminar(idSubcategoria, modificadoPor);
        }

        public static Subcategoria Leer(int idSubcategoria)
        {
            if (idSubcategoria <= 0)
                throw new Exception("ID de subcategoria no valido.");

            Subcategoria sub = SubcategoriaDAL.Consultar(idSubcategoria);
            if (sub == null)
                throw new Exception("Subcategoria no encontrada.");

            return sub;
        }

        public static List<Subcategoria> ListarPorCategoria(int idCategoria)
        {
            if (idCategoria <= 0)
                throw new Exception("ID de categoria no valido.");

            return SubcategoriaDAL.ListarPorCategoria(idCategoria);
        }
    }
}