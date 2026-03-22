using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Validaciones
{
    public class ObligacionFijaValidaciones
    {
        public static void Insertar(ObligacionFija obf, int idUsuario, int creadoPor)
        {
            if (string.IsNullOrEmpty(obf.NombreObligacion))
                throw new Exception("Nombre de obligacion es obligatorio.");
            if (obf.IdSubcategoria <= 0)
                throw new Exception("ID de subcategoria no valido.");
            if (obf.MontoMensual <= 0)
                throw new Exception("Monto mensual debe ser mayor a cero.");
            if (obf.DiaVencimiento < 1 || obf.DiaVencimiento > 31)
                throw new Exception("Dia de vencimiento debe estar entre 1 y 31.");
            if (obf.FechaFin.HasValue && obf.FechaFin.Value <= obf.FechaInicio)
                throw new Exception("Fecha de fin debe ser mayor a la fecha de inicio.");

            ObligacionFijaDAL.Insertar(obf, idUsuario, creadoPor);
        }

        public static void Actualizar(ObligacionFija obf, int modificadoPor)
        {
            if (obf.IdObligacion <= 0)
                throw new Exception("ID de obligacion no valido.");
            if (string.IsNullOrEmpty(obf.NombreObligacion))
                throw new Exception("Nombre de obligacion es obligatorio.");
            if (obf.MontoMensual <= 0)
                throw new Exception("Monto mensual debe ser mayor a cero.");
            if (obf.DiaVencimiento < 1 || obf.DiaVencimiento > 31)
                throw new Exception("Dia de vencimiento debe estar entre 1 y 31.");

            ObligacionFijaDAL.Actualizar(obf, modificadoPor);
        }

        public static void Eliminar(int idObligacion, int modificadoPor)
        {
            if (idObligacion <= 0)
                throw new Exception("ID de obligacion no valido.");

            ObligacionFijaDAL.Eliminar(idObligacion, modificadoPor);
        }

        public static ObligacionFija Leer(int idObligacion)
        {
            if (idObligacion <= 0)
                throw new Exception("ID de obligacion no valido.");

            ObligacionFija obf = ObligacionFijaDAL.Consultar(idObligacion);
            if (obf == null)
                throw new Exception("Obligacion no encontrada.");

            return obf;
        }

        public static List<ObligacionFija> ListarPorUsuario(int idUsuario, bool activo)
        {
            if (idUsuario <= 0)
                throw new Exception("ID de usuario no valido.");

            return ObligacionFijaDAL.ListarPorUsuario(idUsuario, activo);
        }
    }
}