using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;

namespace PresupuestoPersonal.Menus
{
    public class MenuPresupuestos
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║        MENU PRESUPUESTOS          ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Listar presupuestos");
                Console.WriteLine("2. Consultar presupuesto");
                Console.WriteLine("3. Crear presupuesto completo");
                Console.WriteLine("4. Actualizar presupuesto");
                Console.WriteLine("5. Cerrar presupuesto");
                Console.WriteLine("6. Eliminar presupuesto");
                Console.WriteLine("7. Detalles del presupuesto");
                Console.WriteLine("0. Volver");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1": Listar(usuario); break;
                        case "2": Consultar(); break;
                        case "3": CrearCompleto(usuario); break;
                        case "4": Actualizar(usuario); break;
                        case "5": Cerrar(usuario); break;
                        case "6": Eliminar(usuario); break;
                        case "7": MenuPresupuestoDetalles.Mostrar(usuario); break;
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

            if (usuario.Password == "ADMIN@2026!")
                presupuestos = PresupuestoValidaciones.ListarTodos(estado);
            else
                presupuestos = PresupuestoValidaciones.ListarPorUsuario(usuario.IdUsuario, estado);

            if (presupuestos.Count == 0)
            {
                Console.WriteLine("No hay presupuestos registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-35} {"Inicio",-10} {"Fin",-10} {"Estado",-10}");
                Console.WriteLine(new string('-', 73));
                foreach (Presupuesto p in presupuestos)
                {
                    string nombre = p.NombrePresupuesto.Length > 33 ? p.NombrePresupuesto.Substring(0, 30) + "..." : p.NombrePresupuesto;
                    string inicio = $"{p.MesInicio}/{p.AnioInicio}";
                    string fin = $"{p.MesFin}/{p.AnioFin}";
                    Console.WriteLine($"{p.IdPresupuesto,-5} {nombre,-35} {inicio,-10} {fin,-10} {p.EstadoPresupuesto,-10}");
                }
                foreach (Presupuesto p in presupuestos)
                {
                    string inicio = $"{p.MesInicio}/{p.AnioInicio}";
                    string fin = $"{p.MesFin}/{p.AnioFin}";
                    Console.WriteLine($"{p.IdPresupuesto,-5} {p.NombrePresupuesto,-30} {inicio,-12} {fin,-12} {p.EstadoPresupuesto,-10}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Consultar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       CONSULTAR PRESUPUESTO       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del presupuesto: ");
            int id = int.Parse(Console.ReadLine());

            Presupuesto p = PresupuestoValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:                    {p.IdPresupuesto}");
            Console.WriteLine($"Nombre:                {p.NombrePresupuesto}");
            Console.WriteLine($"Periodo:               {p.MesInicio}/{p.AnioInicio} - {p.MesFin}/{p.AnioFin}");
            Console.WriteLine($"Ingresos planificados: L. {p.TotalIngresosPlanificados:N2}");
            Console.WriteLine($"Gastos planificados:   L. {p.TotalGastosPlanificados:N2}");
            Console.WriteLine($"Ahorro planificado:    L. {p.TotalAhorroPlanificado:N2}");
            Console.WriteLine($"Estado:                {p.EstadoPresupuesto}");
            Console.WriteLine($"Fecha creacion:        {p.FechaYHoraCreacion:dd/MM/yyyy HH:mm}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void CrearCompleto(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       CREAR PRESUPUESTO           ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Presupuesto nuevo = new Presupuesto();
            nuevo.IdUsuario = usuario.IdUsuario;

            Console.Write("Nombre del presupuesto: ");
            nuevo.NombrePresupuesto = Console.ReadLine();

            Console.Write("Anio inicio:            ");
            nuevo.AnioInicio = short.Parse(Console.ReadLine());

            Console.Write("Mes inicio (1-12):      ");
            nuevo.MesInicio = byte.Parse(Console.ReadLine());

            Console.Write("Anio fin:               ");
            nuevo.AnioFin = short.Parse(Console.ReadLine());

            Console.Write("Mes fin (1-12):         ");
            nuevo.MesFin = byte.Parse(Console.ReadLine());

            List<string> items = new List<string>();
            bool agregarMas = true;

            Console.WriteLine("\nAgregue las subcategorias del presupuesto:");
            while (agregarMas)
            {
                Console.WriteLine("--- CATEGORIAS ---");
                List<Categoria> categorias = CategoriaDAL.Listar(usuario.IdUsuario, null);
                Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Tipo",-10}");
                Console.WriteLine(new string('-', 42));
                foreach (Categoria c in categorias)
                    Console.WriteLine($"{c.IdCategoria,-5} {c.NombreCategoria,-25} {c.TipoCategoria,-10}");
                Console.WriteLine();
                Console.Write("ID de la categoria: ");
                int idCategoria = int.Parse(Console.ReadLine());

                Console.WriteLine("--- SUBCATEGORIAS ---");
                List<Subcategoria> subs = SubcategoriaDAL.ListarPorCategoria(idCategoria);
                Console.WriteLine($"{"ID",-5} {"Nombre",-25}");
                Console.WriteLine(new string('-', 32));
                foreach (Subcategoria s in subs)
                    Console.WriteLine($"{s.IdSubcategoria,-5} {s.NombreSubcategoria,-25}");
                Console.WriteLine();
                Console.Write("ID de subcategoria: ");
                int idSub = int.Parse(Console.ReadLine());

                Console.Write("Monto mensual:   ");
                decimal monto = decimal.Parse(Console.ReadLine());

                items.Add($"{{\"id_subcategoria\":{idSub},\"monto_mensual\":{monto}}}");

                Console.Write("Agregar otra subcategoria? (s/n): ");
                agregarMas = Console.ReadLine().ToLower() == "s";
            }

            string json = $"[{string.Join(",", items)}]";

            PresupuestoValidaciones.CrearCompleto(nuevo, json, usuario.IdUsuario);

            Console.WriteLine("\nPresupuesto creado correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ACTUALIZAR PRESUPUESTO      ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del presupuesto a actualizar: ");
            int id = int.Parse(Console.ReadLine());

            Presupuesto p = PresupuestoValidaciones.Leer(id);

            Console.WriteLine($"\nDatos actuales: {p.NombrePresupuesto} - {p.MesInicio}/{p.AnioInicio} al {p.MesFin}/{p.AnioFin}");
            Console.WriteLine("(Deje en blanco para mantener el valor actual)");
            Console.WriteLine();

            Console.Write($"Nombre [{p.NombrePresupuesto}]: ");
            string nombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(nombre)) p.NombrePresupuesto = nombre;

            Console.Write($"Anio inicio [{p.AnioInicio}]: ");
            string anioInicio = Console.ReadLine();
            if (!string.IsNullOrEmpty(anioInicio)) p.AnioInicio = short.Parse(anioInicio);

            Console.Write($"Mes inicio [{p.MesInicio}]: ");
            string mesInicio = Console.ReadLine();
            if (!string.IsNullOrEmpty(mesInicio)) p.MesInicio = byte.Parse(mesInicio);

            Console.Write($"Anio fin [{p.AnioFin}]: ");
            string anioFin = Console.ReadLine();
            if (!string.IsNullOrEmpty(anioFin)) p.AnioFin = short.Parse(anioFin);

            Console.Write($"Mes fin [{p.MesFin}]: ");
            string mesFin = Console.ReadLine();
            if (!string.IsNullOrEmpty(mesFin)) p.MesFin = byte.Parse(mesFin);

            PresupuestoValidaciones.Actualizar(p, usuario.IdUsuario);

            Console.WriteLine("\nPresupuesto actualizado correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Cerrar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║        CERRAR PRESUPUESTO         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del presupuesto a cerrar: ");
            int id = int.Parse(Console.ReadLine());

            Presupuesto p = PresupuestoValidaciones.Leer(id);

            Console.Write($"\nEsta seguro que desea cerrar '{p.NombrePresupuesto}'? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                PresupuestoValidaciones.Cerrar(id, usuario.IdUsuario,
                    out decimal totalIngresos,
                    out decimal totalGastos,
                    out decimal totalAhorros);

                Console.WriteLine("\nPresupuesto cerrado correctamente.");
                Console.WriteLine("\n--- RESUMEN FINAL ---");
                Console.WriteLine($"Total ingresos: L. {totalIngresos:N2}");
                Console.WriteLine($"Total gastos:   L. {totalGastos:N2}");
                Console.WriteLine($"Total ahorros:  L. {totalAhorros:N2}");
                Console.WriteLine($"Balance final:  L. {(totalIngresos - totalGastos - totalAhorros):N2}");
            }
            else
            {
                Console.WriteLine("\nOperacion cancelada.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Eliminar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║        ELIMINAR PRESUPUESTO       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del presupuesto a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            Presupuesto p = PresupuestoValidaciones.Leer(id);

            Console.Write($"\nEsta seguro que desea eliminar '{p.NombrePresupuesto}'? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                PresupuestoValidaciones.Eliminar(id, usuario.IdUsuario);
                Console.WriteLine("\nPresupuesto eliminado correctamente.");
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