// MenuPresupuestoDetalles.cs
using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;

namespace PresupuestoPersonal.Menus
{
    public class MenuPresupuestoDetalles
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║     MENU DETALLES PRESUPUESTO     ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Listar detalles de presupuesto");
                Console.WriteLine("2. Consultar detalle");
                Console.WriteLine("3. Insertar detalle");
                Console.WriteLine("4. Actualizar detalle");
                Console.WriteLine("5. Eliminar detalle");
                Console.WriteLine("0. Volver");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            Listar(usuario);
                            break;
                        case "2":
                            Consultar();
                            break;
                        case "3":
                            Insertar(usuario);
                            break;
                        case "4":
                            Actualizar(usuario);
                            break;
                        case "5":
                            Eliminar(usuario);
                            break;
                        case "0":
                            salir = true;
                            break;
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

        // En MenuPresupuestos.cs método Listar
        private static void Listar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       LISTA DE PRESUPUESTOS       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Filtrar por estado (activo/cerrado) o Enter para todos: ");
            string estado = Console.ReadLine();

            List<Presupuesto> presupuestos;

            // Si es admin ve todos, si no solo los suyos
            if (usuario.Password == "ADMIN@2026!")
            {
                presupuestos = PresupuestoValidaciones.ListarTodos(estado);  // nuevo método
            }
            else
            {
                presupuestos = PresupuestoValidaciones.ListarPorUsuario(usuario.IdUsuario, estado);
            }
          
  
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Consultar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║      CONSULTAR DETALLE            ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del detalle: ");
            int id = int.Parse(Console.ReadLine());

            PresupuestoDetalle pd = PresupuestoDetalleValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:               {pd.IdDetalle}");
            Console.WriteLine($"ID Presupuesto:   {pd.IdPresupuesto}");
            Console.WriteLine($"ID Subcategoria:  {pd.IdSubcategoria}");
            Console.WriteLine($"Monto mensual:    L. {pd.MontoMensual:N2}");
            Console.WriteLine($"Observaciones:    {pd.Observaciones ?? "-"}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Insertar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       INSERTAR DETALLE            ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            PresupuestoDetalle nuevo = new PresupuestoDetalle();

            Console.Write("ID del presupuesto:  ");
            nuevo.IdPresupuesto = int.Parse(Console.ReadLine());

            Console.Write("ID de subcategoria:  ");
            nuevo.IdSubcategoria = int.Parse(Console.ReadLine());

            Console.Write("Monto mensual:       ");
            nuevo.MontoMensual = decimal.Parse(Console.ReadLine());

            Console.Write("Observaciones:       ");
            nuevo.Observaciones = Console.ReadLine();

            PresupuestoDetalleValidaciones.Insertar(nuevo, usuario.IdUsuario);

            Console.WriteLine("\nDetalle insertado correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ACTUALIZAR DETALLE          ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del detalle a actualizar: ");
            int id = int.Parse(Console.ReadLine());

            PresupuestoDetalle pd = PresupuestoDetalleValidaciones.Leer(id);

            Console.WriteLine($"\nDatos actuales: Subcategoria {pd.IdSubcategoria} - L. {pd.MontoMensual:N2}");
            Console.WriteLine("(Deje en blanco para mantener el valor actual)");
            Console.WriteLine();

            Console.Write($"Monto mensual [{pd.MontoMensual}]: ");
            string monto = Console.ReadLine();
            if (!string.IsNullOrEmpty(monto)) pd.MontoMensual = decimal.Parse(monto);

            Console.Write($"Observaciones [{pd.Observaciones ?? "-"}]: ");
            string obs = Console.ReadLine();
            if (!string.IsNullOrEmpty(obs)) pd.Observaciones = obs;

            PresupuestoDetalleValidaciones.Actualizar(pd, usuario.IdUsuario);

            Console.WriteLine("\nDetalle actualizado correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Eliminar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ELIMINAR DETALLE            ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del detalle a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            PresupuestoDetalle pd = PresupuestoDetalleValidaciones.Leer(id);

            Console.WriteLine($"\nEsta seguro que desea eliminar el detalle de subcategoria {pd.IdSubcategoria}? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                PresupuestoDetalleValidaciones.Eliminar(id, usuario.IdUsuario);
                Console.WriteLine("\nDetalle eliminado correctamente.");
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