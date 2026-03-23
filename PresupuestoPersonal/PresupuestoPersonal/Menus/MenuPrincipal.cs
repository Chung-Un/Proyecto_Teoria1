using PresupuestoPersonal.Models;

namespace PresupuestoPersonal.Menus
{
    public class MenuPrincipal
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║         MENU PRINCIPAL            ║");
                Console.WriteLine($"║  Usuario: {usuario.PrimerNombre,-22}║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Usuarios");
                Console.WriteLine("2. Categorias");
                Console.WriteLine("3. Subcategorias");
                Console.WriteLine("4. Presupuestos");
                Console.WriteLine("5. Transacciones");
                Console.WriteLine("6. Obligaciones Fijas");
                Console.WriteLine("0. Cerrar Sesion");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            MenuUsuarios.Mostrar(usuario);
                            break;
                        case "2":
                            MenuCategorias.Mostrar(usuario);
                            break;
                        case "3":
                            MenuSubcategorias.Mostrar(usuario);
                            break;
                        case "4":
                            MenuPresupuestos.Mostrar(usuario);
                            break;
                        case "5":
                            MenuTransacciones.Mostrar(usuario);
                            break;
                        case "6":
                            MenuObligaciones.Mostrar(usuario);
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
    }
}