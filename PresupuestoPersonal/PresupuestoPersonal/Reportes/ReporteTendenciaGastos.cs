using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PresupuestoPersonal.Reportes
{
    public class FilaTendenciaGastos
    {
        public short Anio { get; set; }
        public byte Mes { get; set; }
        public string NombreCategoria { get; set; }
        public decimal TotalGastado { get; set; }
    }

    public class ReporteTendenciaGastos
    {
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

        public static void Generar(int idUsuario, string nombreUsuario, short anioInicio, byte mesInicio, short anioFin, byte mesFin)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = LicenseType.Community;

            List<FilaTendenciaGastos> datos = ObtenerDatos(idUsuario, anioInicio, mesInicio, anioFin, mesFin);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte4_TendenciaGastos_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            string[] meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            List<string> categorias = datos.Select(d => d.NombreCategoria).Distinct().OrderBy(c => c).ToList();
            List<(short anio, byte mes)> periodos = datos
                .Select(d => (d.Anio, d.Mes))
                .Distinct()
                .OrderBy(p => p.Anio).ThenBy(p => p.Mes)
                .ToList();

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
                        col.Item().Text("Reporte 4: Tendencia de Gastos por Categoria en el Tiempo").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario} | Periodo: {meses[mesInicio]}/{anioInicio} - {meses[mesFin]}/{anioFin}").FontSize(10).AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });

                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);  
                                foreach (string cat in categorias)
                                    columns.RelativeColumn(3);  
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#3d105c").Padding(5).Text("Mes/Año").FontColor("#ffffff").Bold();
                                foreach (string cat in categorias)
                                    header.Cell().Background("#3d105c").Padding(5).Text(cat).FontColor("#ffffff").Bold().AlignRight();
                            });

                            bool fila = false;
                            foreach ((short anio, byte mes) periodo in periodos)
                            {
                                string bg = fila ? "#F2F2F2" : "#FFFFFF";

                                table.Cell().Background(bg).Padding(5).Text($"{meses[periodo.mes]} {periodo.anio}");
                                foreach (string cat in categorias)
                                {
                                    decimal total = datos
                                        .Where(d => d.Anio == periodo.anio && d.Mes == periodo.mes && d.NombreCategoria == cat)
                                        .Sum(d => d.TotalGastado);
                                    table.Cell().Background(bg).Padding(5).Text($"L. {total:N2}").AlignRight();
                                }

                                fila = !fila;
                            }

     
                            table.Cell().Background("#2C3E50").Padding(5).Text("TOTAL").FontColor("#FFFFFF").Bold();
                            foreach (string cat in categorias)
                            {
                                decimal totalCat = datos.Where(d => d.NombreCategoria == cat).Sum(d => d.TotalGastado);
                                table.Cell().Background("#2C3E50").Padding(5).Text($"L. {totalCat:N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            }
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