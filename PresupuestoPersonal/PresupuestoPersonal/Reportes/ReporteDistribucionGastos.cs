using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PresupuestoPersonal.Reportes
{
    public class FilaDistribucionGastos
    {
        public string NombreCategoria { get; set; }
        public decimal TotalGastado { get; set; }
        public int NumTransacciones { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class ReporteDistribucionGastos
    {
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

        public static void Generar(int idUsuario, string nombreUsuario, short anio, byte mes)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = LicenseType.Community;

            List<FilaDistribucionGastos> datos = ObtenerDatos(idUsuario, anio, mes);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte2_DistribucionGastos_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            string[] meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

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
                        col.Item().Text("Reporte 2: Distribucion de Gastos por Categoria").FontSize(12).AlignCenter();
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
                                columns.RelativeColumn(2);  
                                columns.RelativeColumn(2);  
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#3d105c").Padding(5).Text("Categoria").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Total Gastado").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Transacciones").FontColor("#ffffff").Bold().AlignCenter();
                                header.Cell().Background("#3d105c").Padding(5).Text("Porcentaje").FontColor("#ffffff").Bold().AlignRight();
                            });

                            bool fila = false;
                            foreach (FilaDistribucionGastos f in datos)
                            {
                                string bg = fila ? "#F2F2F2" : "#FFFFFF";

                                table.Cell().Background(bg).Padding(5).Text(f.NombreCategoria);
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalGastado:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"{f.NumTransacciones}").AlignCenter();
                                table.Cell().Background(bg).Padding(5).Text($"{f.Porcentaje:N1}%").AlignRight();

                                fila = !fila;
                            }

                            table.Cell().Background("#2C3E50").Padding(5).Text("TOTAL").FontColor("#FFFFFF").Bold();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.TotalGastado):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"{datos.Sum(d => d.NumTransacciones)}").FontColor("#FFFFFF").Bold().AlignCenter();
                            table.Cell().Background("#2C3E50").Padding(5).Text("100%").FontColor("#FFFFFF").Bold().AlignRight();
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