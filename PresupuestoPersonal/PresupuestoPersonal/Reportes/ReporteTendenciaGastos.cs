using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;
using Color = ScottPlot.Color;

namespace PresupuestoPersonal.Reportes
{
    public class FilaTendenciaGastos
    {
        public short Anio { get; set; }
        public byte Mes { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal TotalGastado { get; set; }
    }

    public class ReporteTendenciaGastos
    {
        private static readonly string[] Meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

        public static List<FilaTendenciaGastos> ObtenerDatos(int idUsuario, short anioInicio, byte mesInicio, short anioFin, byte mesFin)
        {
            List<FilaTendenciaGastos> lista = new List<FilaTendenciaGastos>();
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_reporte_tendencia_gastos", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_anio_inicio", anioInicio);
            cmd.Parameters.AddWithValue("@p_mes_inicio", mesInicio);
            cmd.Parameters.AddWithValue("@p_anio_fin", anioFin);
            cmd.Parameters.AddWithValue("@p_mes_fin", mesFin);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaTendenciaGastos
                {
                    Anio = reader.GetInt16(reader.GetOrdinal("anio")),
                    Mes = reader.GetByte(reader.GetOrdinal("mes")),
                    NombreCategoria = reader.GetString(reader.GetOrdinal("nombre_categoria")),
                    TotalGastado = reader.GetDecimal(reader.GetOrdinal("total_gastado"))
                });
            }
            return lista;
        }

        private static string GenerarGrafico(List<FilaTendenciaGastos> datos)
        {
            string rutaImagen = Path.Combine(Path.GetTempPath(), $"reporte4_{Guid.NewGuid()}.png");
            Plot myPlot = new Plot();
            var categorias = datos.Select(d => d.NombreCategoria).Distinct().OrderBy(c => c).ToList();
            var periodos = datos.Select(d => (d.Anio, d.Mes)).Distinct().OrderBy(p => p.Anio).ThenBy(p => p.Mes).ToList();
            double[] xs = Enumerable.Range(0, periodos.Count).Select(i => (double)i).ToArray();

            foreach (var cat in categorias)
            {
                double[] ys = periodos.Select(p => (double)datos.Where(d => d.Anio == p.Anio && d.Mes == p.Mes && d.NombreCategoria == cat).Sum(d => d.TotalGastado)).ToArray();
                var sig = myPlot.Add.Scatter(xs, ys);
                sig.Label = cat;
                sig.LineWidth = 2;
                sig.MarkerSize = 8;
            }

            Tick[] ticks = periodos.Select((p, i) => new Tick(i, $"{Meses[p.Mes]}\n{p.Anio}")).ToArray();
            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            myPlot.Axes.Left.Label.Text = "Lempiras (L.)";
            myPlot.Title("Tendencia de Gastos por Categoría");
            myPlot.ShowLegend(Edge.Right);
            myPlot.SavePng(rutaImagen, 800, 450);
            return rutaImagen;
        }

        public static void Generar(int idUsuario, string nombreUsuario, short anioInicio, byte mesInicio, short anioFin, byte mesFin)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);
            QuestPDF.Settings.License = LicenseType.Community;
            List<FilaTendenciaGastos> datos = ObtenerDatos(idUsuario, anioInicio, mesInicio, anioFin, mesFin);
            string rutaGrafico = GenerarGrafico(datos);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte4_Tendencia_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            var categorias = datos.Select(d => d.NombreCategoria).Distinct().OrderBy(c => c).ToList();
            var periodos = datos.Select(d => (d.Anio, d.Mes)).Distinct().OrderBy(p => p.Anio).ThenBy(p => p.Mes).ToList();

            Document.Create(container => {
                container.Page(page => {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(8));
                    page.Header().Column(col => {
                        col.Item().Text("GESTOR DE PRESUPUESTOS PERSONALES").FontSize(16).Bold().AlignCenter();
                        col.Item().Text("Reporte 4: Tendencia de Gastos por Categoría").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario} | {mesInicio}/{anioInicio} - {mesFin}/{anioFin}").AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });
                    page.Content().PaddingTop(10).Column(col => {
                        col.Item().AlignCenter().Image(rutaGrafico);
                        col.Item().PaddingTop(10).Table(table => {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn(2);
                                foreach (var c in categorias) columns.RelativeColumn(2);
                            });
                            table.Header(header => {
                                header.Cell().Background("#3d105c").Padding(5).Text("Periodo").FontColor("#fff").Bold();
                                foreach (var cat in categorias) header.Cell().Background("#3d105c").Padding(5).Text(cat).FontColor("#fff").Bold().AlignRight();
                            });
                            bool alternar = false;
                            foreach (var p in periodos)
                            {
                                string bg = alternar ? "#F2F2F2" : "#FFFFFF";
                                table.Cell().Background(bg).Padding(5).Text($"{Meses[p.Mes]} {p.Anio}");
                                foreach (var cat in categorias)
                                {
                                    decimal total = datos.Where(d => d.Anio == p.Anio && d.Mes == p.Mes && d.NombreCategoria == cat).Sum(d => d.TotalGastado);
                                    table.Cell().Background(bg).Padding(5).Text($"L. {total:N2}").AlignRight();
                                }
                                alternar = !alternar;
                            }
                            table.Cell().Background("#2C3E50").Padding(5).Text("TOTAL").FontColor("#fff").Bold();
                            foreach (var cat in categorias)
                            {
                                decimal totalCat = datos.Where(d => d.NombreCategoria == cat).Sum(d => d.TotalGastado);
                                table.Cell().Background("#2C3E50").Padding(5).Text($"L. {totalCat:N2}").FontColor("#fff").Bold().AlignRight();
                            }
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