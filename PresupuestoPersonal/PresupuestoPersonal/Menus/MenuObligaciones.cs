using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;

namespace PresupuestoPersonal.Menus
{
    public class MenuObligaciones
    {
        public static void Mostrar(Usuario usuario)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║      MENU OBLIGACIONES FIJAS      ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Listar obligaciones");
                Console.WriteLine("2. Consultar obligacion");
                Console.WriteLine("3. Insertar obligacion");
                Console.WriteLine("4. Actualizar obligacion");
                Console.WriteLine("5. Eliminar obligacion");
                Console.WriteLine("6. Ver obligaciones del mes");
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
                        case "3": Insertar(usuario); break;
                        case "4": Actualizar(usuario); break;
                        case "5": Eliminar(usuario); break;
                        case "6": VerObligacionesMes(usuario); break;
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
            Console.WriteLine("║     LISTA DE OBLIGACIONES FIJAS   ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("Mostrar activas (s/n): ");
            bool activo = Console.ReadLine().ToLower() == "s";

            List<ObligacionFija> obligaciones = ObligacionFijaValidaciones.ListarPorUsuario(usuario.IdUsuario, activo);

            if (obligaciones.Count == 0)
            {
                Console.WriteLine("No hay obligaciones registradas.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Monto",-14} {"Dia Vence",-10} {"Vigente",-8}");
                Console.WriteLine(new string('-', 65));
                foreach (ObligacionFija obf in obligaciones)
                    Console.WriteLine($"{obf.IdObligacion,-5} {obf.NombreObligacion,-25} L.{obf.MontoMensual,-12:N2} {obf.DiaVencimiento,-10} {(obf.EstadoVigente ? "Si" : "No"),-8}");
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Consultar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       CONSULTAR OBLIGACION        ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la obligacion: ");
            int id = int.Parse(Console.ReadLine());

            ObligacionFija obf = ObligacionFijaValidaciones.Leer(id);

            Console.WriteLine();
            Console.WriteLine($"ID:              {obf.IdObligacion}");
            Console.WriteLine($"Nombre:          {obf.NombreObligacion}");
            Console.WriteLine($"ID Subcategoria: {obf.IdSubcategoria}");
            Console.WriteLine($"Monto mensual:   L. {obf.MontoMensual:N2}");
            Console.WriteLine($"Dia vencimiento: {obf.DiaVencimiento}");
            Console.WriteLine($"Fecha inicio:    {obf.FechaInicio}");
            Console.WriteLine($"Fecha fin:       {obf.FechaFin?.ToString() ?? "Indefinida"}");
            Console.WriteLine($"Estado vigente:  {(obf.EstadoVigente ? "Activa" : "Inactiva")}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Insertar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       INSERTAR OBLIGACION         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            ObligacionFija nueva = new ObligacionFija();

            Console.Write("Nombre:                    ");
            nueva.NombreObligacion = Console.ReadLine();

            Console.Write("ID de subcategoria:        ");
            nueva.IdSubcategoria = int.Parse(Console.ReadLine());

            Console.Write("Monto mensual:             ");
            nueva.MontoMensual = decimal.Parse(Console.ReadLine());

            Console.Write("Dia de vencimiento (1-31): ");
            nueva.DiaVencimiento = byte.Parse(Console.ReadLine());

            Console.Write("Fecha inicio (dd/MM/yyyy): ");
            nueva.FechaInicio = DateOnly.ParseExact(Console.ReadLine(), "dd/MM/yyyy");

            Console.Write("Fecha fin (dd/MM/yyyy) o Enter si es indefinida: ");
            string fechaFin = Console.ReadLine();
            nueva.FechaFin = string.IsNullOrEmpty(fechaFin) ? null : DateOnly.ParseExact(fechaFin, "dd/MM/yyyy");

            nueva.EstadoVigente = true;

            ObligacionFijaValidaciones.Insertar(nueva, usuario.IdUsuario, usuario.IdUsuario);

            Console.WriteLine("\nObligacion insertada correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Actualizar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ACTUALIZAR OBLIGACION       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la obligacion a actualizar: ");
            int id = int.Parse(Console.ReadLine());

            ObligacionFija obf = ObligacionFijaValidaciones.Leer(id);

            Console.WriteLine($"\nDatos actuales: {obf.NombreObligacion} - L. {obf.MontoMensual:N2} - Dia {obf.DiaVencimiento}");
            Console.WriteLine("(Deje en blanco para mantener el valor actual)");
            Console.WriteLine();

            Console.Write($"Nombre [{obf.NombreObligacion}]: ");
            string nombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(nombre)) obf.NombreObligacion = nombre;

            Console.Write($"Monto mensual [{obf.MontoMensual}]: ");
            string monto = Console.ReadLine();
            if (!string.IsNullOrEmpty(monto)) obf.MontoMensual = decimal.Parse(monto);

            Console.Write($"Dia vencimiento [{obf.DiaVencimiento}]: ");
            string dia = Console.ReadLine();
            if (!string.IsNullOrEmpty(dia)) obf.DiaVencimiento = byte.Parse(dia);

            Console.Write($"Fecha fin [{obf.FechaFin?.ToString() ?? "Indefinida"}] (dd/MM/yyyy): ");
            string fechaFin = Console.ReadLine();
            if (!string.IsNullOrEmpty(fechaFin)) obf.FechaFin = DateOnly.ParseExact(fechaFin, "dd/MM/yyyy");

            Console.Write($"Vigente [{(obf.EstadoVigente ? "s" : "n")}] (s/n): ");
            string vigente = Console.ReadLine();
            if (!string.IsNullOrEmpty(vigente)) obf.EstadoVigente = vigente.ToLower() == "s";

            ObligacionFijaValidaciones.Actualizar(obf, usuario.IdUsuario);

            Console.WriteLine("\nObligacion actualizada correctamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void Eliminar(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       ELIMINAR OBLIGACION         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("ID de la obligacion a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            ObligacionFija obf = ObligacionFijaValidaciones.Leer(id);

            Console.Write($"\nEsta seguro que desea eliminar '{obf.NombreObligacion}'? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                ObligacionFijaValidaciones.Eliminar(id, usuario.IdUsuario);
                Console.WriteLine("\nObligacion eliminada correctamente.");
            }
            else
            {
                Console.WriteLine("\nOperacion cancelada.");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void VerObligacionesMes(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║       OBLIGACIONES DEL MES        ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("Anio:               ");
            short anio = short.Parse(Console.ReadLine());

            Console.Write("Mes (1-12):         ");
            byte mes = byte.Parse(Console.ReadLine());

            Console.Write("ID del presupuesto: ");
            int idPresupuesto = int.Parse(Console.ReadLine());

            using Microsoft.Data.SqlClient.SqlConnection conexion = DataAccess.Conexion.ObtenerConexion();
            using Microsoft.Data.SqlClient.SqlCommand cmd = new("sp_procesar_obligaciones_mes", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", usuario.IdUsuario);
            cmd.Parameters.AddWithValue("@p_anio", anio);
            cmd.Parameters.AddWithValue("@p_mes", mes);
            cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);

            using Microsoft.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine();
            Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Monto",-14} {"Vence",-12} {"Dias",-10} {"Pagada",-8}");
            Console.WriteLine(new string('-', 76));

            bool hayDatos = false;
            while (reader.Read())
            {
                hayDatos = true;
                int idObligacion = reader.GetInt32(reader.GetOrdinal("id_obligacion"));
                string nombre = reader.GetString(reader.GetOrdinal("nombre_obligacion"));
                decimal monto = reader.GetDecimal(reader.GetOrdinal("monto_mensual"));
                DateTime fechaVence = reader.GetDateTime(reader.GetOrdinal("fecha_vencimiento_mes"));
                int diasParaVencer = reader.GetInt32(reader.GetOrdinal("dias_hasta_vencer"));
                bool yaPagada = reader.GetInt32(reader.GetOrdinal("ya_pagada")) == 1;

                string estadoPago = yaPagada ? "Si" : "No";
                string alertaDias = diasParaVencer < 0 ? "VENCIDA"
                                  : diasParaVencer <= 3 ? "URGENTE"
                                  : $"{diasParaVencer} dias";

                Console.WriteLine($"{idObligacion,-5} {nombre,-25} L.{monto,-12:N2} {fechaVence:dd/MM/yyyy,-12} {alertaDias,-10} {estadoPago,-8}");
            }

            if (!hayDatos)
                Console.WriteLine("No hay obligaciones para este mes.");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}