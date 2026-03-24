using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;

namespace PresupuestoPersonal.Menus
{
    public class MenuUsuarios
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║           MENU USUARIOS          ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Listar usuarios");
                Console.WriteLine("2. Consultar usuario");
                Console.WriteLine("3. Insertar usuario");
                Console.WriteLine("4. Actualizar usuario");
                Console.WriteLine("5. Eliminar usuario");
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
                            Eliminar();
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
            Console.WriteLine("║          LISTA DE USUARIOS        ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            List<Usuario> usuarios = UsuarioValidaciones.Listar();

            if (usuarios.Count == 0)
            {
                Console.WriteLine("No hay usuarios registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Correo",-30} {"Estado",-10}");
                Console.WriteLine(new string('-', 70));
                foreach (Usuario u in usuarios)
                {
                    Console.WriteLine($"{u.IdUsuario,-5} {u.PrimerNombre,-25} {u.CorreoElectronico,-30} {(u.EstadoUsuario ? "Activo" : "Inactivo"),-10}");
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
            Console.WriteLine("║         CONSULTAR USUARIO         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del usuario: ");
            int id = int.Parse(Console.ReadLine());

            Usuario u = UsuarioValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:               {u.IdUsuario}");
            Console.WriteLine($"Nombre:           {u.PrimerNombre} {u.SegundoNombre} {u.PrimerApellido} {u.SegundoApellido}");
            Console.WriteLine($"Correo:           {u.CorreoElectronico}");
            Console.WriteLine($"Salario base:     L. {u.SalarioMensualBase:N2}");
            Console.WriteLine($"Fecha ingreso:    {u.FechaIngreso:dd/MM/yyyy}");
            Console.WriteLine($"Estado:           {(u.EstadoUsuario ? "Activo" : "Inactivo")}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Insertar(Usuario usuarioActivo)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║         INSERTAR USUARIO          ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Usuario nuevo = new Usuario();

            Console.Write("Primer nombre:     ");
            nuevo.PrimerNombre = Console.ReadLine();

            Console.Write("Segundo nombre:    ");
            nuevo.SegundoNombre = Console.ReadLine();

            Console.Write("Primer apellido:   ");
            nuevo.PrimerApellido = Console.ReadLine();

            Console.Write("Segundo apellido:  ");
            nuevo.SegundoApellido = Console.ReadLine();

            Console.Write("Correo:            ");
            nuevo.CorreoElectronico = Console.ReadLine();

            Console.Write("Contrasena:        ");
            nuevo.Password = Console.ReadLine();

            Console.Write("Salario mensual:   ");
            nuevo.SalarioMensualBase = decimal.Parse(Console.ReadLine());

            nuevo.IdUsuario = usuarioActivo.IdUsuario;

            UsuarioValidaciones.Insertar(nuevo);

            Console.WriteLine("\nUsuario insertado correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuarioActivo)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║         ACTUALIZAR USUARIO        ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del usuario a actualizar: ");
            int id = int.Parse(Console.ReadLine());

            Usuario u = UsuarioValidaciones.Leer(id);

            Console.WriteLine($"\nDatos actuales: {u.PrimerNombre} {u.PrimerApellido} - L. {u.SalarioMensualBase:N2}");
            Console.WriteLine("(Deje en blanco para mantener el valor actual)");
            Console.WriteLine();

            Console.Write($"Primer nombre [{u.PrimerNombre}]: ");
            string primerNombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(primerNombre)) u.PrimerNombre = primerNombre;

            Console.Write($"Segundo nombre [{u.SegundoNombre}]: ");
            string segundoNombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(segundoNombre)) u.SegundoNombre = segundoNombre;

            Console.Write($"Primer apellido [{u.PrimerApellido}]: ");
            string primerApellido = Console.ReadLine();
            if (!string.IsNullOrEmpty(primerApellido)) u.PrimerApellido = primerApellido;

            Console.Write($"Segundo apellido [{u.SegundoApellido}]: ");
            string segundoApellido = Console.ReadLine();
            if (!string.IsNullOrEmpty(segundoApellido)) u.SegundoApellido = segundoApellido;

            Console.Write($"Salario mensual [{u.SalarioMensualBase}]: ");
            string salario = Console.ReadLine();
            if (!string.IsNullOrEmpty(salario)) u.SalarioMensualBase = decimal.Parse(salario);

            UsuarioValidaciones.Actualizar(u);

            Console.WriteLine("\nUsuario actualizado correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Eliminar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║          ELIMINAR USUARIO         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID del usuario a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            Usuario u = UsuarioValidaciones.Leer(id);

            Console.WriteLine($"\nEsta seguro que desea eliminar a {u.PrimerNombre} {u.PrimerApellido}? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                UsuarioValidaciones.Eliminar(id);
                Console.WriteLine("\nUsuario eliminado correctamente.");
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