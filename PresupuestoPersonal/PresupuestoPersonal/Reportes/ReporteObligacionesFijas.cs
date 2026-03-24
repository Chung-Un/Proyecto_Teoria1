using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;
using Color = ScottPlot.Color;

namespace PresupuestoPersonal.Reportes
{
    public class FilaObligacionFija
    {
        public string NombreObligacion { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal MontoMensual { get; set; }
        public int DiaVencimiento { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int DiasHastaVencer { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
    }

    public class ReporteObligacionesFijas
    {
        private static readonly string[] Meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

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

        private static string GenerarGrafico(List<FilaObligacionFija> datos)
        {
            string rutaImagen = Path.Combine(Path.GetTempPath(), $"reporte5_{Guid.NewGuid()}.png");
            Plot myPlot = new Plot(); 

            var counts = new double[] {
        datos.Count(d => d.EstadoPago == "Pagada"),
        datos.Count(d => d.EstadoPago == "Pendiente"),
        datos.Count(d => d.EstadoPago == "Por vencer"),
        datos.Count(d => d.EstadoPago == "Vencida")
    };

            List<PieSlice> slices = new() {
        new() { Value = counts[0], FillColor = ScottPlot.Color.FromHex("#27AE60"), Label = $"Pagadas ({counts[0]})" },
        new() { Value = counts[1], FillColor = ScottPlot.Color.FromHex("#F39C12"), Label = $"Pendientes ({counts[1]})" },
        new() { Value = counts[2], FillColor = ScottPlot.Color.FromHex("#E67E22"), Label = $"Por vencer ({counts[2]})" },
        new() { Value = counts[3], FillColor = ScottPlot.Color.FromHex("#E74C3C"), Label = $"Vencidas ({counts[3]})" }
    };

            var pie = myPlot.Add.Pie(slices);
            pie.ExplodeFraction = 0.1;

            myPlot.Title("Resumen de Obligaciones");
            myPlot.Axes.Frameless(); 

            myPlot.SavePng(rutaImagen, 500, 400);
            return rutaImagen;
        }

        public static void Generar(int idUsuario, string nombreUsuario, short anio, byte mes)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);
            QuestPDF.Settings.License = LicenseType.Community;
            List<FilaObligacionFija> datos = ObtenerDatos(idUsuario, anio, mes);
            string rutaGrafico = GenerarGrafico(datos);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte5_Obligaciones_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            Document.Create(container => {
                container.Page(page => {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9));
                    page.Header().Column(col => {
                        col.Item().Text("GESTOR DE PRESUPUESTOS PERSONALES").FontSize(16).Bold().AlignCenter();
                        col.Item().Text("Reporte 5: Estado de Obligaciones Fijas").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario} | Periodo: {Meses[mes]} {anio}").AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });
                    page.Content().PaddingTop(10).Row(row => {
                        row.RelativeItem(2).Column(izq => {
                            izq.Item().Text("Detalle de Obligaciones").Bold().FontSize(11);
                            izq.Item().PaddingTop(5).Table(table => {
                                table.ColumnsDefinition(columns => {
                                    columns.RelativeColumn(4); columns.RelativeColumn(3); columns.RelativeColumn(2);
                                    columns.RelativeColumn(1); columns.RelativeColumn(2); columns.RelativeColumn(2);
                                });
                                table.Header(header => {
                                    header.Cell().Background("#3d105c").Padding(4).Text("Obligación").FontColor("#fff").Bold();
                                    header.Cell().Background("#3d105c").Padding(4).Text("Categoría").FontColor("#fff").Bold();
                                    header.Cell().Background("#3d105c").Padding(4).Text("Monto").FontColor("#fff").Bold().AlignRight();
                                    header.Cell().Background("#3d105c").Padding(4).Text("Día").FontColor("#fff").Bold().AlignCenter();
                                    header.Cell().Background("#3d105c").Padding(4).Text("Vence en").FontColor("#fff").Bold().AlignCenter();
                                    header.Cell().Background("#3d105c").Padding(4).Text("Estado").FontColor("#fff").Bold().AlignCenter();
                                });
                                bool alternar = false;
                                foreach (var f in datos)
                                {
                                    string bg = alternar ? "#F2F2F2" : "#FFFFFF";
                                    string eCol = f.EstadoPago switch { "Pagada" => "#27AE60", "Pendiente" => "#F39C12", "Por vencer" => "#E67E22", "Vencida" => "#E74C3C", _ => "#000" };
                                    table.Cell().Background(bg).Padding(4).Text(f.NombreObligacion);
                                    table.Cell().Background(bg).Padding(4).Text(f.NombreCategoria);
                                    table.Cell().Background(bg).Padding(4).Text($"L. {f.MontoMensual:N2}").AlignRight();
                                    table.Cell().Background(bg).Padding(4).Text($"{f.DiaVencimiento}").AlignCenter();
                                    table.Cell().Background(bg).Padding(4).Text(f.DiasHastaVencer < 0 ? $"{Math.Abs(f.DiasHastaVencer)}d atraso" : $"{f.DiasHastaVencer}d").AlignCenter();
                                    table.Cell().Background(bg).Padding(4).Text(f.EstadoPago).FontColor(eCol).Bold().AlignCenter();
                                    alternar = !alternar;
                                }
                            });
                        });
                        row.ConstantItem(15);
                        row.RelativeItem(1).Column(der => {
                            der.Item().Text("Resumen Gráfico").Bold().FontSize(11);
                            der.Item().PaddingTop(5).Image(rutaGrafico);
                        });
                    });
                    page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            }).GeneratePdf(rutaPDF);

            if (File.Exists(rutaGrafico)) File.Delete(rutaGrafico);
            Console.WriteLine($"\nReporte generado: {rutaPDF}");
            Console.ReadKey();
        }
    }
}