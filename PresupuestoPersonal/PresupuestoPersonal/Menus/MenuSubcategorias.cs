using PresupuestoPersonal.DataAccess;
using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;

namespace PresupuestoPersonal.Menus
{
    public class MenuSubcategorias
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║       MENU SUBCATEGORIAS          ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Listar subcategorias por categoria");
                Console.WriteLine("2. Consultar subcategoria");
                Console.WriteLine("3. Insertar subcategoria");
                Console.WriteLine("4. Actualizar subcategoria");
                Console.WriteLine("5. Eliminar subcategoria");
                Console.WriteLine("0. Volver");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            Listar();
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

        private static void Listar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║      LISTA DE SUBCATEGORIAS       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la categoria: ");
            int idCategoria = int.Parse(Console.ReadLine());

            List<Subcategoria> subcategorias = SubcategoriaValidaciones.ListarPorCategoria(idCategoria);

            if (subcategorias.Count == 0)
            {
                Console.WriteLine("No hay subcategorias registradas.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Defecto",-10} {"Estado",-10}");
                Console.WriteLine(new string('-', 55));
                foreach (Subcategoria s in subcategorias)
                {
                    Console.WriteLine($"{s.IdSubcategoria,-5} {s.NombreSubcategoria,-25} {(s.SubcategoriaPorDefecto ? "Si" : "No"),-10} {(s.EstadoSubcategoria ? "Activa" : "Inactiva"),-10}");
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
            Console.WriteLine("║       CONSULTAR SUBCATEGORIA      ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la subcategoria: ");
            int id = int.Parse(Console.ReadLine());

            Subcategoria s = SubcategoriaValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:               {s.IdSubcategoria}");
            Console.WriteLine($"ID Categoria:     {s.IdCategoria}");
            Console.WriteLine($"Nombre:           {s.NombreSubcategoria}");
            Console.WriteLine($"Por defecto:      {(s.SubcategoriaPorDefecto ? "Si" : "No")}");
            Console.WriteLine($"Estado:           {(s.EstadoSubcategoria ? "Activa" : "Inactiva")}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Insertar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       INSERTAR SUBCATEGORIA       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Subcategoria nueva = new Subcategoria();

            Console.WriteLine("--- CATEGORIAS ---");
            List<Categoria> categorias = CategoriaDAL.Listar(usuario.IdUsuario, null);
            Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Tipo",-10}");
            Console.WriteLine(new string('-', 42));
            foreach (Categoria c in categorias)
                Console.WriteLine($"{c.IdCategoria,-5} {c.NombreCategoria,-25} {c.TipoCategoria,-10}");
            Console.WriteLine();
            Console.Write("ID de la categoria: ");
            int idCategoria = int.Parse(Console.ReadLine());

            Console.Write("Nombre:             ");
            nueva.NombreSubcategoria = Console.ReadLine();

            nueva.EstadoSubcategoria = true;
            nueva.SubcategoriaPorDefecto = false;

            SubcategoriaValidaciones.Insertar(nueva, usuario.IdUsuario);

            Console.WriteLine("\nSubcategoria insertada correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ACTUALIZAR SUBCATEGORIA     ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la subcategoria a actualizar: ");
            int id = int.Parse(Console.ReadLine());

            Subcategoria s = SubcategoriaValidaciones.Leer(id);

            Console.WriteLine($"\nDatos actuales: {s.NombreSubcategoria}");
            Console.WriteLine("(Deje en blanco para mantener el valor actual)");
            Console.WriteLine();

            Console.Write($"Nombre [{s.NombreSubcategoria}]: ");
            string nombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(nombre)) s.NombreSubcategoria = nombre;

            SubcategoriaValidaciones.Actualizar(s, usuario.IdUsuario);

            Console.WriteLine("\nSubcategoria actualizada correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Eliminar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ELIMINAR SUBCATEGORIA       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la subcategoria a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            Subcategoria s = SubcategoriaValidaciones.Leer(id);

            Console.WriteLine($"\nEsta seguro que desea eliminar '{s.NombreSubcategoria}'? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                SubcategoriaValidaciones.Eliminar(id, usuario.IdUsuario);
                Console.WriteLine("\nSubcategoria eliminada correctamente.");
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