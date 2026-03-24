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
                Console.WriteLine("2. Reporte 2 - Distribucion de Gastos");
                Console.WriteLine("3. Reporte 3 - Cumplimiento de Presupuesto");
                Console.WriteLine("4. Reporte 4 - Tendencia de Gastos");
                Console.WriteLine("5. Reporte 5 - Obligaciones Fijas");
                Console.WriteLine("6. Reporte 6 - Progreso de Ahorros");
                Console.WriteLine("0. Volver");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1": GenerarReporte1(usuario); break;
                        case "2": GenerarReporte2(usuario); break;
                        case "3": GenerarReporte3(usuario); break;
                        case "4": GenerarReporte4(usuario); break;
                        case "5": GenerarReporte5(usuario); break;
                        case "6": GenerarReporte6(usuario); break;
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

        private static void GenerarReporte1(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║    REPORTE 1 - RESUMEN MENSUAL    ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Anio inicio:       "); short anioInicio = short.Parse(Console.ReadLine());
            Console.Write("Mes inicio (1-12): "); byte mesInicio = byte.Parse(Console.ReadLine());
            Console.Write("Anio fin:          "); short anioFin = short.Parse(Console.ReadLine());
            Console.Write("Mes fin (1-12):    "); byte mesFin = byte.Parse(Console.ReadLine());
            ReporteResumenMensual.Generar(usuario.IdUsuario, $"{usuario.PrimerNombre} {usuario.PrimerApellido}", anioInicio, mesInicio, anioFin, mesFin);
        }

        private static void GenerarReporte2(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║  REPORTE 2 - DISTRIBUCION GASTOS  ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Anio:        "); short anio = short.Parse(Console.ReadLine());
            Console.Write("Mes (1-12):  "); byte mes = byte.Parse(Console.ReadLine());
            ReporteDistribucionGastos.Generar(usuario.IdUsuario, $"{usuario.PrimerNombre} {usuario.PrimerApellido}", anio, mes);
        }

        private static void GenerarReporte3(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║ REPORTE 3 - CUMPLIMIENTO PRESUP.  ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("ID del presupuesto: "); int idPresupuesto = int.Parse(Console.ReadLine());
            Console.Write("Anio:               "); short anio = short.Parse(Console.ReadLine());
            Console.Write("Mes (1-12):         "); byte mes = byte.Parse(Console.ReadLine());
            ReporteCumplimientoPresupuesto.Generar(usuario.IdUsuario, $"{usuario.PrimerNombre} {usuario.PrimerApellido}", idPresupuesto, anio, mes);
        }

        private static void GenerarReporte4(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║  REPORTE 4 - TENDENCIA GASTOS     ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Anio inicio:       "); short anioInicio = short.Parse(Console.ReadLine());
            Console.Write("Mes inicio (1-12): "); byte mesInicio = byte.Parse(Console.ReadLine());
            Console.Write("Anio fin:          "); short anioFin = short.Parse(Console.ReadLine());
            Console.Write("Mes fin (1-12):    "); byte mesFin = byte.Parse(Console.ReadLine());
            ReporteTendenciaGastos.Generar(usuario.IdUsuario, $"{usuario.PrimerNombre} {usuario.PrimerApellido}", anioInicio, mesInicio, anioFin, mesFin);
        }

        private static void GenerarReporte5(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║  REPORTE 5 - OBLIGACIONES FIJAS   ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Anio:        "); short anio = short.Parse(Console.ReadLine());
            Console.Write("Mes (1-12):  "); byte mes = byte.Parse(Console.ReadLine());
            ReporteObligacionesFijas.Generar(usuario.IdUsuario, $"{usuario.PrimerNombre} {usuario.PrimerApellido}", anio, mes);
        }

        private static void GenerarReporte6(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║  REPORTE 6 - PROGRESO AHORROS     ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();
            ReporteProgresoAhorros.Generar(usuario.IdUsuario, $"{usuario.PrimerNombre} {usuario.PrimerApellido}");
        }
    }
}