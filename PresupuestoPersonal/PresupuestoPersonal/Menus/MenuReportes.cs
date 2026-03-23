using PresupuestoPersonal.Models;
using PresupuestoPersonal.Reportes;

namespace PresupuestoPersonal.Menus
{
    public class MenuReportes
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║          MENU REPORTES            ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Reporte 1 - Resumen Mensual");
                Console.WriteLine("0. Volver");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            GenerarReporte1(usuario);
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

        private static void GenerarReporte1(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║    REPORTE 1 - RESUMEN MENSUAL    ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("Anio inicio:       ");
            short anioInicio = short.Parse(Console.ReadLine());

            Console.Write("Mes inicio (1-12): ");
            byte mesInicio = byte.Parse(Console.ReadLine());

            Console.Write("Anio fin:          ");
            short anioFin = short.Parse(Console.ReadLine());

            Console.Write("Mes fin (1-12):    ");
            byte mesFin = byte.Parse(Console.ReadLine());

            string nombreUsuario = $"{usuario.PrimerNombre} {usuario.PrimerApellido}";

            ReporteResumenMensual.Generar(usuario.IdUsuario, nombreUsuario, anioInicio, mesInicio, anioFin, mesFin);
        }
    }
}
