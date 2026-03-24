using PresupuestoPersonal.DataAccess;
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
                        case "1": Listar(); break;
                        case "2": Consultar(); break;
                        case "3": Insertar(usuario); break;
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
            Console.WriteLine("║    LISTA DE DETALLES PRESUPUESTO  ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del presupuesto: ");
            int idPresupuesto = int.Parse(Console.ReadLine());

            List<PresupuestoDetalle> detalles = PresupuestoDetalleValidaciones.ListarPorPresupuesto(idPresupuesto);

            if (detalles.Count == 0)
            {
                Console.WriteLine("No hay detalles para este presupuesto.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"ID Sub",-10} {"Monto Mensual",-16} {"Observaciones",-30}");
                Console.WriteLine(new string('-', 65));
                foreach (PresupuestoDetalle pd in detalles)
                {
                    string obs = pd.Observaciones ?? "-";
                    if (obs.Length > 28) obs = obs.Substring(0, 25) + "...";
                    Console.WriteLine($"{pd.IdDetalle,-5} {pd.IdSubcategoria,-10} L.{pd.MontoMensual,-14:N2} {obs,-30}");
                }
                foreach (PresupuestoDetalle pd in detalles)
                    Console.WriteLine($"{pd.IdDetalle,-5} {pd.IdSubcategoria,-18} L.{pd.MontoMensual,-14:N2} {pd.Observaciones ?? "-",-20}");
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Consultar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║         CONSULTAR DETALLE         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del detalle: ");
            int id = int.Parse(Console.ReadLine());

            PresupuestoDetalle pd = PresupuestoDetalleValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:              {pd.IdDetalle}");
            Console.WriteLine($"ID Presupuesto:  {pd.IdPresupuesto}");
            Console.WriteLine($"ID Subcategoria: {pd.IdSubcategoria}");
            Console.WriteLine($"Monto mensual:   L. {pd.MontoMensual:N2}");
            Console.WriteLine($"Observaciones:   {pd.Observaciones ?? "-"}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Insertar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║         INSERTAR DETALLE         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            PresupuestoDetalle nuevo = new PresupuestoDetalle();

            Console.Write("ID del presupuesto: ");
            nuevo.IdPresupuesto = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("--- CATEGORIAS DISPONIBLES ---");
            List<Categoria> categorias = CategoriaDAL.Listar(usuario.IdUsuario, null);
            Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Tipo",-10}");
            Console.WriteLine(new string('-', 42));
            foreach (Categoria c in categorias)
                Console.WriteLine($"{c.IdCategoria,-5} {c.NombreCategoria,-25} {c.TipoCategoria,-10}");

            Console.WriteLine();
            Console.Write("ID de la categoria: ");
            int idCategoria = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("--- SUBCATEGORIAS DISPONIBLES ---");
            List<Subcategoria> subcategorias = SubcategoriaDAL.ListarPorCategoria(idCategoria);
            Console.WriteLine($"{"ID",-5} {"Nombre",-25}");
            Console.WriteLine(new string('-', 32));
            foreach (Subcategoria s in subcategorias)
                Console.WriteLine($"{s.IdSubcategoria,-5} {s.NombreSubcategoria,-25}");

            Console.WriteLine();
            Console.Write("ID de subcategoria: ");
            nuevo.IdSubcategoria = int.Parse(Console.ReadLine());

            Console.Write("Monto mensual:      ");
            nuevo.MontoMensual = decimal.Parse(Console.ReadLine());

            Console.Write("Observaciones (Enter para omitir): ");
            string obs = Console.ReadLine();
            nuevo.Observaciones = string.IsNullOrEmpty(obs) ? null : obs;

            PresupuestoDetalleValidaciones.Insertar(nuevo, usuario.IdUsuario);

            Console.WriteLine("\nDetalle insertado correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║         ACTUALIZAR DETALLE        ║");
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
            Console.WriteLine("║         ELIMINAR DETALLE          ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del detalle a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            PresupuestoDetalle pd = PresupuestoDetalleValidaciones.Leer(id);

            Console.Write($"\nEsta seguro que desea eliminar el detalle de subcategoria {pd.IdSubcategoria}? (s/n): ");
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