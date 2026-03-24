using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;
using Color = ScottPlot.Color;

namespace PresupuestoPersonal.Reportes
{
    public class FilaDistribucionGastos
    {
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal TotalGastado { get; set; }
        public int NumTransacciones { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class ReporteDistribucionGastos
    {
        private static readonly string[] Meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

        public static List<FilaDistribucionGastos> ObtenerDatos(int idUsuario, short anio, byte mes)
        {
            List<FilaDistribucionGastos> lista = new List<FilaDistribucionGastos>();
            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_reporte_distribucion_gastos", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_anio", anio);
            cmd.Parameters.AddWithValue("@p_mes", mes);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaDistribucionGastos
                {
                    NombreCategoria = reader.GetString(reader.GetOrdinal("nombre_categoria")),
                    TotalGastado = reader.GetDecimal(reader.GetOrdinal("total_gastado")),
                    NumTransacciones = reader.GetInt32(reader.GetOrdinal("num_transacciones")),
                    Porcentaje = reader.GetDecimal(reader.GetOrdinal("porcentaje"))
                });
            }
            return lista;
        }

        private static string GenerarGrafico(List<FilaDistribucionGastos> datos)
        {
            string rutaImagen = Path.Combine(Path.GetTempPath(), $"reporte2_{Guid.NewGuid()}.png");
            Plot myPlot = new Plot();

            var slices = datos.Select(d => new PieSlice
            {
                Value = (double)d.TotalGastado,
                Label = d.NombreCategoria
            }).ToList();

            var pie = myPlot.Add.Pie(slices);
            pie.DonutFraction = 0.5; 

            myPlot.Title("Distribución de Gastos");

            myPlot.Axes.Frameless();
            myPlot.HideGrid();

            myPlot.SavePng(rutaImagen, 700, 400);
            return rutaImagen;
        }

        public static void Generar(int idUsuario, string nombreUsuario, short anio, byte mes)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);
            QuestPDF.Settings.License = LicenseType.Community;
            List<FilaDistribucionGastos> datos = ObtenerDatos(idUsuario, anio, mes);
            string rutaGrafico = GenerarGrafico(datos);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte2_DistribucionGastos_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
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
                        col.Item().Text("Reporte 2: Distribución de Gastos por Categoría").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario} | Periodo: {Meses[mes]} {anio}").FontSize(10).AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });
                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Text("Gráfico de Distribución").Bold().FontSize(11);
                        col.Item().PaddingTop(5).Image(rutaGrafico);
                        col.Item().PaddingTop(10).LineHorizontal(1);
                        col.Item().PaddingTop(10).Text("Tabla de Datos").Bold().FontSize(11);
                        col.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn(4); columns.RelativeColumn(3);
                                columns.RelativeColumn(2); columns.RelativeColumn(2);
                            });
                            table.Header(header => {
                                header.Cell().Background("#3d105c").Padding(5).Text("Categoría").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Total Gastado").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Transacciones").FontColor("#ffffff").Bold().AlignCenter();
                                header.Cell().Background("#3d105c").Padding(5).Text("Porcentaje").FontColor("#ffffff").Bold().AlignRight();
                            });
                            bool alternar = false;
                            foreach (var f in datos)
                            {
                                string bg = alternar ? "#F2F2F2" : "#FFFFFF";
                                table.Cell().Background(bg).Padding(5).Text(f.NombreCategoria);
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalGastado:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"{f.NumTransacciones}").AlignCenter();
                                table.Cell().Background(bg).Padding(5).Text($"{f.Porcentaje:N1}%").AlignRight();
                                alternar = !alternar;
                            }
                            table.Cell().Background("#2C3E50").Padding(5).Text("TOTAL").FontColor("#ffffff").Bold();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.TotalGastado):N2}").FontColor("#ffffff").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"{datos.Sum(d => d.NumTransacciones)}").FontColor("#ffffff").Bold().AlignCenter();
                            table.Cell().Background("#2C3E50").Padding(5).Text("100%").FontColor("#ffffff").Bold().AlignRight();
                        });
                    });
                    page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8);
                });
            }).GeneratePdf(rutaPDF);
            if (File.Exists(rutaGrafico)) File.Delete(rutaGrafico);
            Console.WriteLine($"\nReporte generado: {rutaPDF}");
            Console.ReadKey();
        }
    }
}