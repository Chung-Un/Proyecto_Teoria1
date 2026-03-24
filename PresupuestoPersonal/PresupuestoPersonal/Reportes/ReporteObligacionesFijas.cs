using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PresupuestoPersonal.Reportes
{
    public class FilaObligacionFija
    {
        public string NombreObligacion { get; set; }
        public string NombreCategoria { get; set; }
        public decimal MontoMensual { get; set; }
        public int DiaVencimiento { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int DiasHastaVencer { get; set; }
        public string EstadoPago { get; set; }
    }

    public class ReporteObligacionesFijas
    {
        public static List<FilaObligacionFija> ObtenerDatos(int idUsuario, short anio, byte mes)
        {
            List<FilaObligacionFija> lista = new List<FilaObligacionFija>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_reporte_obligaciones_fijas", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_anio", anio);
            cmd.Parameters.AddWithValue("@p_mes", mes);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaObligacionFija
                {
                    NombreObligacion = reader.GetString(reader.GetOrdinal("nombre_obligacion")),
                    NombreCategoria = reader.GetString(reader.GetOrdinal("nombre_categoria")),
                    MontoMensual = reader.GetDecimal(reader.GetOrdinal("monto_mensual")),
                    DiaVencimiento = reader.GetByte(reader.GetOrdinal("dia_vencimiento")),
                    FechaVencimiento = reader.GetDateTime(reader.GetOrdinal("fecha_vencimiento")),
                    DiasHastaVencer = reader.GetInt32(reader.GetOrdinal("dias_hasta_vencer")),
                    EstadoPago = reader.GetString(reader.GetOrdinal("estado_pago"))
                });
            }
            return lista;
        }

        public static void Generar(int idUsuario, string nombreUsuario, short anio, byte mes)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = LicenseType.Community;

            List<FilaObligacionFija> datos = ObtenerDatos(idUsuario, anio, mes);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte5_ObligacionesFijas_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            string[] meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("GESTOR DE PRESUPUESTOS PERSONALES").FontSize(16).Bold().AlignCenter();
                        col.Item().Text("Reporte 5: Estado de Obligaciones Fijas y Cumplimiento de Pagos").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario} | Periodo: {meses[mes]} {anio}").FontSize(10).AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });

                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4); 
                                columns.RelativeColumn(3); 
                                columns.RelativeColumn(3);  
                                columns.RelativeColumn(2); 
                                columns.RelativeColumn(2);  
                                columns.RelativeColumn(2);  
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#3d105c").Padding(5).Text("Obligacion").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Categoria").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Monto").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Dia Vence").FontColor("#ffffff").Bold().AlignCenter();
                                header.Cell().Background("#3d105c").Padding(5).Text("Dias").FontColor("#ffffff").Bold().AlignCenter();
                                header.Cell().Background("#3d105c").Padding(5).Text("Estado").FontColor("#ffffff").Bold().AlignCenter();
                            });

                            bool fila = false;
                            foreach (FilaObligacionFija f in datos)
                            {
                                string bg = fila ? "#F2F2F2" : "#FFFFFF";

                                string estadoColor = f.EstadoPago switch
                                {
                                    "Pagada" => "#27AE60",
                                    "Pendiente" => "#F39C12",
                                    "Por vencer" => "#E67E22",
                                    "Vencida" => "#E74C3C",
                                    _ => "#000000"
                                };

                                string diasTexto = f.DiasHastaVencer < 0
                                    ? $"{Math.Abs(f.DiasHastaVencer)} dias atras"
                                    : $"{f.DiasHastaVencer} dias";

                                table.Cell().Background(bg).Padding(5).Text(f.NombreObligacion);
                                table.Cell().Background(bg).Padding(5).Text(f.NombreCategoria);
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.MontoMensual:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"Dia {f.DiaVencimiento}").AlignCenter();
                                table.Cell().Background(bg).Padding(5).Text(diasTexto).AlignCenter();
                                table.Cell().Background(bg).Padding(5).Text(f.EstadoPago).FontColor(estadoColor).Bold().AlignCenter();

                                fila = !fila;
                            }

                            table.Cell().Background("#2C3E50").Padding(5).Text("TOTAL").FontColor("#FFFFFF").Bold();
                            table.Cell().Background("#2C3E50").Padding(5).Text("").FontColor("#FFFFFF");
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.MontoMensual):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text("").FontColor("#FFFFFF");
                            table.Cell().Background("#2C3E50").Padding(5).Text("").FontColor("#FFFFFF");
                            table.Cell().Background("#2C3E50").Padding(5)
                                .Text($"{datos.Count(d => d.EstadoPago == "Pagada")} pagadas / {datos.Count(d => d.EstadoPago != "Pagada")} pendientes")
                                .FontColor("#FFFFFF").Bold().AlignCenter();
                        });
                    });

                    page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8);
                });
            }).GeneratePdf(rutaPDF);

            Console.WriteLine($"\nReporte generado: {rutaPDF}");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}