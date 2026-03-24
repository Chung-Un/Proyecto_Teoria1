using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;  
using PresupuestoPersonal.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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

        public static void Generar(int idUsuario, string nombreUsuario, short anioInicio, byte mesInicio, short anioFin, byte mesFin)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = LicenseType.Community;

            List<FilaResumenMensual> datos = ObtenerDatos(idUsuario, anioInicio, mesInicio, anioFin, mesFin);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte1_ResumenMensual_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

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
                        col.Item().Text("Reporte 1: Resumen Mensual de ingresos vs gastos vs ahorros").FontSize(12).AlignCenter();  
                        col.Item().Text($"Usuario: {nombreUsuario} | Periodo {mesInicio}/{anioInicio} - {mesFin}/{anioFin}").FontSize(10).AlignCenter(); 
                    });

                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#3d105c").Padding(5).Text("Mes/Año").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Ingresos").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Gastos").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Ahorros").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Otros").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Balance").FontColor("#ffffff").Bold();
                            });

                            foreach (FilaResumenMensual f in datos)
                            {
                                string bg = "#FFFFFF";  
                                string balanceColor = f.BalanceFinal >= 0 ? "#27AE60" : "#e0402f";  
                                string[] meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                                        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

                                table.Cell().Background(bg).Padding(5).Text($"{meses[f.Mes]} {f.Anio}");
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalIngresos:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalGastos:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalAhorros:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.TotalOtros:N2}").AlignRight();  
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.BalanceFinal:N2}").FontColor(balanceColor).AlignRight();
                            }

                            table.Cell().Background("#2C3E50").Padding(5).Text("TOTAL").FontColor("#FFFFFF").Bold();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.TotalIngresos):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.TotalGastos):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.TotalAhorros):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.TotalOtros):N2}").FontColor("#FFFFFF").Bold().AlignRight(); 
                            decimal balanceTotal = datos.Sum(d => d.BalanceFinal);
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {balanceTotal:N2}").FontColor(balanceTotal >= 0 ? "#2ECC71" : "#E74C3C").Bold().AlignRight();  
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