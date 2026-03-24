using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Validaciones
{
    public class PresupuestoValidaciones
    {
        public static void Insertar(Presupuesto p)
        {
            if (string.IsNullOrEmpty(p.NombrePresupuesto))
                throw new Exception("Nombre de presupuesto es obligatorio.");
            if (p.AnioInicio <= 0 || p.AnioFin <= 0)
                throw new Exception("Anio de inicio y fin son obligatorios.");
            if (p.MesInicio < 1 || p.MesInicio > 12)
                throw new Exception("Mes de inicio no valido.");
            if (p.MesFin < 1 || p.MesFin > 12)
                throw new Exception("Mes de fin no valido.");
            if (p.AnioFin < p.AnioInicio)
                throw new Exception("Anio de fin debe ser mayor o igual al anio de inicio.");
            if (p.AnioFin == p.AnioInicio && p.MesFin < p.MesInicio)
                throw new Exception("Mes de fin debe ser mayor o igual al mes de inicio.");

            PresupuestoDAL.Insertar(p);
        }

        public static void Actualizar(Presupuesto p, int modificadoPor)
        {
            if (p.IdPresupuesto <= 0)
                throw new Exception("ID de presupuesto no valido.");
            if (string.IsNullOrEmpty(p.NombrePresupuesto))
                throw new Exception("Nombre de presupuesto es obligatorio.");
            if (p.AnioFin < p.AnioInicio)
                throw new Exception("Anio de fin debe ser mayor o igual al anio de inicio.");
            if (p.AnioFin == p.AnioInicio && p.MesFin < p.MesInicio)
                throw new Exception("Mes de fin debe ser mayor o igual al mes de inicio.");

            PresupuestoDAL.Actualizar(p, modificadoPor);
        }

        public static void Eliminar(int idPresupuesto, int modificadoPor)
        {
            if (idPresupuesto <= 0)
                throw new Exception("ID de presupuesto no valido.");

            PresupuestoDAL.Eliminar(idPresupuesto, modificadoPor);
        }

        public static Presupuesto Leer(int idPresupuesto)
        {
            if (idPresupuesto <= 0)
                throw new Exception("ID de presupuesto no valido.");

            Presupuesto p = PresupuestoDAL.Consultar(idPresupuesto);
            if (p == null)
                throw new Exception("Presupuesto no encontrado.");

            return p;
        }

        public static List<Presupuesto> ListarPorUsuario(int idUsuario, string estado)
        {
            if (idUsuario <= 0)
                throw new Exception("ID de usuario no valido.");

            return PresupuestoDAL.ListarPorUsuario(idUsuario, estado);
        }

        public static List<Presupuesto> ListarTodos(string estado)
        {
            return PresupuestoDAL.ListarTodos(estado);
        }

        public static void CrearCompleto(Presupuesto p, string listaJson, int creadoPor)
        {
            if (string.IsNullOrEmpty(p.NombrePresupuesto))
                throw new Exception("Nombre de presupuesto es obligatorio.");
            if (string.IsNullOrEmpty(listaJson))
                throw new Exception("Debe incluir al menos una subcategoria.");
            if (p.AnioFin < p.AnioInicio)
                throw new Exception("Anio de fin debe ser mayor o igual al anio de inicio.");
            if (p.AnioFin == p.AnioInicio && p.MesFin < p.MesInicio)
                throw new Exception("Mes de fin debe ser mayor o igual al mes de inicio.");

            PresupuestoDAL.CrearCompleto(p, listaJson, creadoPor);
        }

        public static void Cerrar(int idPresupuesto, int modificadoPor,
                                  out decimal totalIngresos, out decimal totalGastos, out decimal totalAhorros)
        {
            if (idPresupuesto <= 0)
                throw new Exception("ID de presupuesto no valido.");

            PresupuestoDAL.Cerrar(idPresupuesto, modificadoPor, out totalIngresos, out totalGastos, out totalAhorros);
        }
    }
}