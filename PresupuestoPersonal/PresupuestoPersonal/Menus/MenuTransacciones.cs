using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;

namespace PresupuestoPersonal.Menus
{
    public class MenuTransacciones
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║       MENU TRANSACCIONES          ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Listar transacciones");
                Console.WriteLine("2. Consultar transaccion");
                Console.WriteLine("3. Registrar transaccion");
                Console.WriteLine("4. Actualizar transaccion");
                Console.WriteLine("5. Eliminar transaccion");
                Console.WriteLine("0. Volver");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1": Listar(); break;
                        case "2": Consultar(); break;
                        case "3": Registrar(usuario); break;
                        case "4": Actualizar(usuario); break;
                        case "5": Eliminar(usuario); break;
                        case "0": salir = true; break;
                        default:
                            Console.WriteLine("Opcion no valida.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void Listar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║      LISTA DE TRANSACCIONES       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del presupuesto:                        ");
            int idPresupuesto = int.Parse(Console.ReadLine());

            Console.Write("Anio:                                      ");
            short anio = short.Parse(Console.ReadLine());

            Console.Write("Mes (1-12):                                ");
            byte mes = byte.Parse(Console.ReadLine());

            Console.Write("Tipo (ingreso/gasto/ahorro) o Enter todos: ");
            string tipo = Console.ReadLine();

            List<Transaccion> transacciones = TransaccionValidaciones.ListarPorPresupuesto(idPresupuesto, anio, mes, tipo);

            if (transacciones.Count == 0)
            {
                Console.WriteLine("No hay transacciones registradas.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Fecha",-12} {"Tipo",-10} {"Descripcion",-25} {"Monto",-12} {"Metodo",-12}");
                Console.WriteLine(new string('-', 80));
                foreach (Transaccion t in transacciones)
                    Console.WriteLine($"{t.IdTransaccion,-5} {t.FechaTransaccion,-12} {t.TipoTransaccion,-10} {t.DescripcionMovimiento,-25} L.{t.MontoTransaccion,-11:N2} {t.MetodoPago,-12}");
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Consultar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       CONSULTAR TRANSACCION       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la transaccion: ");
            int id = int.Parse(Console.ReadLine());

            Transaccion t = TransaccionValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:            {t.IdTransaccion}");
            Console.WriteLine($"Fecha:         {t.FechaTransaccion}");
            Console.WriteLine($"Anio/Mes:      {t.AnioTransaccion}/{t.MesTransaccion}");
            Console.WriteLine($"Tipo:          {t.TipoTransaccion}");
            Console.WriteLine($"Descripcion:   {t.DescripcionMovimiento}");
            Console.WriteLine($"Monto:         L. {t.MontoTransaccion:N2}");
            Console.WriteLine($"Metodo pago:   {t.MetodoPago}");
            Console.WriteLine($"No. Factura:   {t.NumeroFactura?.ToString() ?? "-"}");
            Console.WriteLine($"Observaciones: {t.Observaciones ?? "-"}");
            Console.WriteLine($"Registrada:    {t.FechaYHoraRegistro:dd/MM/yyyy HH:mm}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Registrar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       REGISTRAR TRANSACCION       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Transaccion nueva = new Transaccion();
            nueva.IdUsuario = usuario.IdUsuario;

            Console.Write("ID del presupuesto:                                          ");
            int idPresupuesto = int.Parse(Console.ReadLine());

            Console.Write("ID de subcategoria:                                          ");
            nueva.IdSubcategoria = int.Parse(Console.ReadLine());

            Console.Write("Tipo (ingreso/gasto/ahorro):                                 ");
            nueva.TipoTransaccion = Console.ReadLine();

            Console.Write("Descripcion:                                                 ");
            nueva.DescripcionMovimiento = Console.ReadLine();

            Console.Write("Monto:                                                       ");
            nueva.MontoTransaccion = decimal.Parse(Console.ReadLine());

            Console.Write("Fecha (dd/MM/yyyy):                                          ");
            nueva.FechaTransaccion = DateOnly.ParseExact(Console.ReadLine(), "dd/MM/yyyy");

            nueva.AnioTransaccion = (short)nueva.FechaTransaccion.Year;
            nueva.MesTransaccion = (byte)nueva.FechaTransaccion.Month;

            Console.Write("Metodo pago (efectivo/tarjeta_debito/tarjeta_credito/transferencia): ");
            nueva.MetodoPago = Console.ReadLine();

            Console.Write("Numero factura (Enter para omitir):                          ");
            string factura = Console.ReadLine();
            nueva.NumeroFactura = string.IsNullOrEmpty(factura) ? null : int.Parse(factura);

            Console.Write("Observaciones (Enter para omitir):                           ");
            nueva.Observaciones = Console.ReadLine();
            if (string.IsNullOrEmpty(nueva.Observaciones)) nueva.Observaciones = null;

            nueva.IdDetalle = idPresupuesto;

            TransaccionValidaciones.RegistrarCompleta(nueva, usuario.IdUsuario);

            Console.WriteLine("\nTransaccion registrada correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ACTUALIZAR TRANSACCION      ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la transaccion a actualizar: ");
            int id = int.Parse(Console.ReadLine());

            Transaccion t = TransaccionValidaciones.Leer(id);

            Console.WriteLine($"\nDatos actuales: {t.DescripcionMovimiento} - L. {t.MontoTransaccion:N2}");
            Console.WriteLine("(Deje en blanco para mantener el valor actual)");
            Console.WriteLine();

            Console.Write($"Descripcion [{t.DescripcionMovimiento}]: ");
            string desc = Console.ReadLine();
            if (!string.IsNullOrEmpty(desc)) t.DescripcionMovimiento = desc;

            Console.Write($"Monto [{t.MontoTransaccion}]: ");
            string monto = Console.ReadLine();
            if (!string.IsNullOrEmpty(monto)) t.MontoTransaccion = decimal.Parse(monto);

            Console.Write($"Fecha [{t.FechaTransaccion}] (dd/MM/yyyy): ");
            string fecha = Console.ReadLine();
            if (!string.IsNullOrEmpty(fecha))
            {
                t.FechaTransaccion = DateOnly.ParseExact(fecha, "dd/MM/yyyy");
                t.AnioTransaccion = (short)t.FechaTransaccion.Year;
                t.MesTransaccion = (byte)t.FechaTransaccion.Month;
            }

            Console.Write($"Metodo pago [{t.MetodoPago}]: ");
            string metodo = Console.ReadLine();
            if (!string.IsNullOrEmpty(metodo)) t.MetodoPago = metodo;

            Console.Write($"Observaciones [{t.Observaciones ?? "-"}]: ");
            string obs = Console.ReadLine();
            if (!string.IsNullOrEmpty(obs)) t.Observaciones = obs;

            TransaccionValidaciones.Actualizar(t, usuario.IdUsuario);

            Console.WriteLine("\nTransaccion actualizada correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Eliminar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ELIMINAR TRANSACCION        ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la transaccion a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            Transaccion t = TransaccionValidaciones.Leer(id);

            Console.Write($"\nEsta seguro que desea eliminar '{t.DescripcionMovimiento} - L. {t.MontoTransaccion:N2}'? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                TransaccionValidaciones.Eliminar(id, usuario.IdUsuario);
                Console.WriteLine("\nTransaccion eliminada correctamente.");
            }
            else
            {
                Console.WriteLine("\nOperacion cancelada.");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}