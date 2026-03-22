using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Validaciones
{
    public class TransaccionValidaciones
    {
        public static void Insertar(Transaccion t)
        {
            if (t.IdUsuario <= 0)
                throw new Exception("ID de usuario no valido.");
            if (t.IdDetalle <= 0)
                throw new Exception("ID de detalle no valido.");
            if (string.IsNullOrEmpty(t.TipoTransaccion))
                throw new Exception("Tipo de transaccion es obligatorio.");
            if (t.TipoTransaccion != "ingreso" && t.TipoTransaccion != "gasto" && t.TipoTransaccion != "ahorro")
                throw new Exception("Tipo de transaccion debe ser ingreso, gasto o ahorro.");
            if (t.MontoTransaccion <= 0)
                throw new Exception("Monto debe ser mayor a cero.");
            if (t.MesTransaccion < 1 || t.MesTransaccion > 12)
                throw new Exception("Mes no valido.");

            TransaccionDAL.Insertar(t);
        }

        public static void Actualizar(Transaccion t, int modificadoPor)
        {
            if (t.IdTransaccion <= 0)
                throw new Exception("ID de transaccion no valido.");
            if (t.MontoTransaccion <= 0)
                throw new Exception("Monto debe ser mayor a cero.");
            if (t.MesTransaccion < 1 || t.MesTransaccion > 12)
                throw new Exception("Mes no valido.");

            TransaccionDAL.Actualizar(t, modificadoPor);
        }

        public static void Eliminar(int idTransaccion, int modificadoPor)
        {
            if (idTransaccion <= 0)
                throw new Exception("ID de transaccion no valido.");

            TransaccionDAL.Eliminar(idTransaccion, modificadoPor);
        }

        public static Transaccion Leer(int idTransaccion)
        {
            if (idTransaccion <= 0)
                throw new Exception("ID de transaccion no valido.");

            Transaccion t = TransaccionDAL.Consultar(idTransaccion);
            if (t == null)
                throw new Exception("Transaccion no encontrada.");

            return t;
        }

        public static List<Transaccion> ListarPorPresupuesto(int idPresupuesto, short anio, byte mes, string tipo)
        {
            if (idPresupuesto <= 0)
                throw new Exception("ID de presupuesto no valido.");
            if (mes < 1 || mes > 12)
                throw new Exception("Mes no valido.");

            return TransaccionDAL.ListarPorPresupuesto(idPresupuesto, anio, mes, tipo);
        }

        public static void RegistrarCompleta(Transaccion t, int creadoPor)
        {
            if (t.IdUsuario <= 0)
                throw new Exception("ID de usuario no valido.");
            if (t.MontoTransaccion <= 0)
                throw new Exception("Monto debe ser mayor a cero.");
            if (t.MesTransaccion < 1 || t.MesTransaccion > 12)
                throw new Exception("Mes no valido.");
            if (string.IsNullOrEmpty(t.TipoTransaccion))
                throw new Exception("Tipo de transaccion es obligatorio.");

            TransaccionDAL.RegistrarCompleta(t, creadoPor);
        }
    }
}