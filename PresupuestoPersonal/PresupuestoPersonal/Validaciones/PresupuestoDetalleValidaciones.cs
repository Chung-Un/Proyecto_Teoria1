using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Validaciones
{
    public class PresupuestoDetalleValidaciones
    {
        public static void Insertar(PresupuestoDetalle pd, int creadoPor)
        {
            if (pd.IdPresupuesto <= 0)
                throw new Exception("ID de presupuesto no valido.");
            if (pd.IdSubcategoria <= 0)
                throw new Exception("ID de subcategoria no valido.");
            if (pd.MontoMensual <= 0)
                throw new Exception("Monto mensual debe ser mayor a cero.");

            PresupuestoDetalleDAL.Insertar(pd, creadoPor);
        }

        public static void Actualizar(PresupuestoDetalle pd, int modificadoPor)
        {
            if (pd.IdDetalle <= 0)
                throw new Exception("ID de detalle no valido.");
            if (pd.MontoMensual <= 0)
                throw new Exception("Monto mensual debe ser mayor a cero.");

            PresupuestoDetalleDAL.Actualizar(pd, modificadoPor);
        }

        public static void Eliminar(int idDetalle, int modificadoPor)
        {
            if (idDetalle <= 0)
                throw new Exception("ID de detalle no valido.");

            PresupuestoDetalleDAL.Eliminar(idDetalle, modificadoPor);
        }

        public static PresupuestoDetalle Leer(int idDetalle)
        {
            if (idDetalle <= 0)
                throw new Exception("ID de detalle no valido.");

            PresupuestoDetalle pd = PresupuestoDetalleDAL.Consultar(idDetalle);
            if (pd == null)
                throw new Exception("Detalle de presupuesto no encontrado.");

            return pd;
        }

        public static List<PresupuestoDetalle> ListarPorPresupuesto(int idPresupuesto)
        {
            if (idPresupuesto <= 0)
                throw new Exception("ID de presupuesto no valido.");

            return PresupuestoDetalleDAL.ListarPorPresupuesto(idPresupuesto);
        }
    }
}