using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;

namespace PresupuestoPersonal.Menus
{
    public class MenuCategorias
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║         MENU CATEGORIAS           ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Listar categorias");
                Console.WriteLine("2. Consultar categoria");
                Console.WriteLine("3. Insertar categoria");
                Console.WriteLine("4. Actualizar categoria");
                Console.WriteLine("5. Eliminar categoria");
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

        private static void Listar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║        LISTA DE CATEGORIAS        ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("Filtrar por tipo (ingreso/gasto/ahorro) o Enter para todas: ");
            string tipo = Console.ReadLine();

            List<Categoria> categorias = CategoriaValidaciones.Listar(usuario.IdUsuario, tipo);

            if (categorias.Count == 0)
            {
                Console.WriteLine("No hay categorias registradas.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Tipo",-10} {"Orden",-6}");
                Console.WriteLine(new string('-', 50));
                foreach (Categoria c in categorias)
                {
                    Console.WriteLine($"{c.IdCategoria,-5} {c.NombreCategoria,-25} {c.TipoCategoria,-10} {c.OrdenPresentacion,-6}");
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
            Console.WriteLine("║        CONSULTAR CATEGORIA        ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la categoria: ");
            int id = int.Parse(Console.ReadLine());

            Categoria c = CategoriaValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:               {c.IdCategoria}");
            Console.WriteLine($"Nombre:           {c.NombreCategoria}");
            Console.WriteLine($"Tipo:             {c.TipoCategoria}");
            Console.WriteLine($"Icono:            {c.NombreIcono}");
            Console.WriteLine($"Color:            {c.ColorHexadecimal}");
            Console.WriteLine($"Orden:            {c.OrdenPresentacion}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Insertar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║        INSERTAR CATEGORIA         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Categoria nueva = new Categoria();

            Console.Write("Nombre: ");
            nueva.NombreCategoria = Console.ReadLine();

            Console.Write("Tipo (ingreso/gasto/ahorro): ");
            nueva.TipoCategoria = Console.ReadLine();

            CategoriaValidaciones.Insertar(nueva);  

            Console.WriteLine("\nCategoria insertada correctamente.");
            Console.WriteLine("(El trigger creo automaticamente la subcategoria General)");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║        ACTUALIZAR CATEGORIA       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la categoria a actualizar: ");
            int id = int.Parse(Console.ReadLine());

            Categoria c = CategoriaValidaciones.Leer(id);

            Console.WriteLine($"\nDatos actuales: {c.NombreCategoria} - {c.TipoCategoria}");
            Console.WriteLine("(Deje en blanco para mantener el valor actual)");
            Console.WriteLine();

            Console.Write($"Nombre [{c.NombreCategoria}]: ");
            string nombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(nombre)) c.NombreCategoria = nombre;

            Console.Write($"Icono [{c.NombreIcono}]: ");
            string icono = Console.ReadLine();
            if (!string.IsNullOrEmpty(icono)) c.NombreIcono = icono;

            Console.Write($"Color [{c.ColorHexadecimal}]: ");
            string color = Console.ReadLine();
            if (!string.IsNullOrEmpty(color)) c.ColorHexadecimal = color;

            Console.Write($"Orden [{c.OrdenPresentacion}]: ");
            string orden = Console.ReadLine();
            if (!string.IsNullOrEmpty(orden)) c.OrdenPresentacion = int.Parse(orden);

            CategoriaValidaciones.Actualizar(c, usuario.IdUsuario);

            Console.WriteLine("\nCategoria actualizada correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Eliminar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║        ELIMINAR CATEGORIA         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la categoria a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            Categoria c = CategoriaValidaciones.Leer(id);

            Console.WriteLine($"\nEsta seguro que desea eliminar '{c.NombreCategoria}'? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                CategoriaValidaciones.Eliminar(id, usuario.IdUsuario);
                Console.WriteLine("\nCategoria eliminada correctamente.");
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