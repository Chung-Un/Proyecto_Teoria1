using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;
using Color = ScottPlot.Color;

namespace PresupuestoPersonal.Reportes
{
    public class FilaResumenMensual
    {
        public short Anio { get; set; }
        public byte Mes { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal TotalAhorros { get; set; }
        public decimal TotalOtros { get; set; }
        public decimal BalanceFinal { get; set; }
    }

    public class ReporteResumenMensual
    {
        private static readonly string[] Meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

        public static List<FilaResumenMensual> ObtenerDatos(int idUsuario, short anioInicio, byte mesInicio, short anioFin, byte mesFin)
        {
            List<FilaResumenMensual> lista = new List<FilaResumenMensual>();
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_reporte_resumen_mensual", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_anio_inicio", anioInicio);
            cmd.Parameters.AddWithValue("@p_mes_inicio", mesInicio);
            cmd.Parameters.AddWithValue("@p_anio_fin", anioFin);
            cmd.Parameters.AddWithValue("@p_mes_fin", mesFin);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaResumenMensual
                {
                    Anio = reader.GetInt16(reader.GetOrdinal("anio")),
                    Mes = reader.GetByte(reader.GetOrdinal("mes")),
                    TotalIngresos = reader.GetDecimal(reader.GetOrdinal("total_ingresos")),
                    TotalGastos = reader.GetDecimal(reader.GetOrdinal("total_gastos")),
                    TotalAhorros = reader.GetDecimal(reader.GetOrdinal("total_ahorros")),
                    TotalOtros = reader.GetDecimal(reader.GetOrdinal("otros")),
                    BalanceFinal = reader.GetDecimal(reader.GetOrdinal("balance_final"))
                });
            }
            return lista;
        }

        private static string GenerarGrafico(List<FilaResumenMensual> datos)
        {
            string rutaImagen = Path.Combine(Path.GetTempPath(), $"reporte1_{Guid.NewGuid()}.png");
            Plot myPlot = new Plot();

            for (int i = 0; i < datos.Count; i++)
            {
                double posBase = i * 4;
                myPlot.Add.Bar(new Bar { Value = (double)datos[i].TotalIngresos, Position = posBase, FillColor = Color.FromHex("#27AE60"), Label = i == 0 ? "Ingresos" : "" });
                myPlot.Add.Bar(new Bar { Value = (double)datos[i].TotalGastos, Position = posBase + 1, FillColor = Color.FromHex("#E74C3C"), Label = i == 0 ? "Gastos" : "" });
                myPlot.Add.Bar(new Bar { Value = (double)datos[i].TotalAhorros, Position = posBase + 2, FillColor = Color.FromHex("#3498DB"), Label = i == 0 ? "Ahorros" : "" });
            }

            Tick[] ticks = datos.Select((d, i) => new Tick(i * 4 + 1, $"{Meses[d.Mes]}\n{d.Anio}")).ToArray();
            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            myPlot.Axes.Left.Label.Text = "Lempiras (L.)";
            myPlot.Title("Resumen Mensual");
            myPlot.ShowLegend();
            myPlot.SavePng(rutaImagen, 700, 400);
            return rutaImagen;
        }

        public static void Generar(int idUsuario, string nombreUsuario, short anioInicio, byte mesInicio, short anioFin, byte mesFin)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);
            QuestPDF.Settings.License = LicenseType.Community;
            List<FilaResumenMensual> datos = ObtenerDatos(idUsuario, anioInicio, mesInicio, anioFin, mesFin);
            string rutaGrafico = GenerarGrafico(datos);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte_Resumen_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    page.Header().Column(col =>
                    {
                        col.Item().Text("GESTOR DE PRESUPUESTOS PERSONALES").FontSize(16).Bold().AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario} | Periodo: {mesInicio}/{anioInicio} - {mesFin}/{anioFin}").AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });
                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Image(rutaGrafico);
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn(2); columns.RelativeColumn(3); columns.RelativeColumn(3);
                                columns.RelativeColumn(3); columns.RelativeColumn(3); columns.RelativeColumn(3);
                            });
                            table.Header(header => {
                                header.Cell().Background("#3d105c").Padding(5).Text("Mes/Año").FontColor("#fff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Ingresos").FontColor("#fff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Gastos").FontColor("#fff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Ahorros").FontColor("#fff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Otros").FontColor("#fff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Balance").FontColor("#fff").Bold().AlignRight();
                            });
                            bool alternar = false;
                            foreach (var f in datos)
                            {
                                string bg = alternar ? "#F2F2F2" : "#FFFFFF";
                                table.Cell().Background(bg).Padding(5).Text($"{Meses[f.Mes]} {f.Anio}");
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalIngresos:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalGastos:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalAhorros:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalOtros:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.BalanceFinal:N2}").FontColor(f.BalanceFinal >= 0 ? "#27AE60" : "#E74C3C").AlignRight();
                                alternar = !alternar;
                            }
                        });
                    });
                    page.Footer().AlignCenter().Text(x => { x.Span("Generado el "); x.Span(DateTime.Now.ToString("dd/MM/yyyy")); });
                });
            }).GeneratePdf(rutaPDF);

            if (File.Exists(rutaGrafico)) File.Delete(rutaGrafico);
        }
    }
}