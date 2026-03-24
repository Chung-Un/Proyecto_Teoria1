using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PresupuestoPersonal.Reportes
{
    public class FilaProgresoAhorro
    {
        public string NombreMeta { get; set; }
        public decimal AhorroMensual { get; set; }
        public decimal MontoAcumulado { get; set; }
        public decimal PorcentajeCompletado { get; set; }
        public decimal MontoObjetivoAnual { get; set; }
    }

    public class ReporteProgresoAhorros
    {
        public static List<FilaProgresoAhorro> ObtenerDatos(int idUsuario)
        {
            List<FilaProgresoAhorro> lista = new List<FilaProgresoAhorro>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_reporte_progreso_ahorros", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new FilaProgresoAhorro
                {
                    NombreMeta = reader.GetString(reader.GetOrdinal("nombre_meta")),
                    AhorroMensual = reader.GetDecimal(reader.GetOrdinal("ahorro_mensual")),
                    MontoAcumulado = reader.GetDecimal(reader.GetOrdinal("monto_acumulado")),
                    PorcentajeCompletado = reader.GetDecimal(reader.GetOrdinal("porcentaje_completado")),
                    MontoObjetivoAnual = reader.GetDecimal(reader.GetOrdinal("monto_objetivo_anual"))
                });
            }
            return lista;
        }

        public static void Generar(int idUsuario, string nombreUsuario)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = LicenseType.Community;

            List<FilaProgresoAhorro> datos = ObtenerDatos(idUsuario);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte6_ProgresoAhorros_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

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
                        col.Item().Text("Reporte 6: Progreso de Metas de Ahorro").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario}").FontSize(10).AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });

                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);  
                                columns.RelativeColumn(3);  
                                columns.RelativeColumn(3);  
                                columns.RelativeColumn(2);  
                                columns.RelativeColumn(3);  
                                columns.RelativeColumn(3);  
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#3d105c").Padding(5).Text("Meta").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Objetivo Anual").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Acumulado").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("%").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Ahorro/Mes").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Estado").FontColor("#ffffff").Bold().AlignCenter();
                            });

                            bool fila = false;
                            foreach (FilaProgresoAhorro f in datos)
                            {
                                string bg = fila ? "#F2F2F2" : "#FFFFFF";

                                string estadoColor;
                                string estadoTexto;
                                if (f.PorcentajeCompletado >= 100)
                                {
                                    estadoColor = "#27AE60";
                                    estadoTexto = "COMPLETADA";
                                }
                                else if (f.PorcentajeCompletado >= 50)
                                {
                                    estadoColor = "#F39C12";
                                    estadoTexto = "EN PROGRESO";
                                }
                                else
                                {
                                    estadoColor = "#E74C3C";
                                    estadoTexto = "INICIANDO";
                                }

                                table.Cell().Background(bg).Padding(5).Text(f.NombreMeta);
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.MontoObjetivoAnual:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.MontoAcumulado:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"{f.PorcentajeCompletado:N1}%").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.AhorroMensual:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text(estadoTexto).FontColor(estadoColor).Bold().AlignCenter();

                                fila = !fila;
                            }

                            table.Cell().Background("#2C3E50").Padding(5).Text("TOTAL").FontColor("#FFFFFF").Bold();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.MontoObjetivoAnual):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.MontoAcumulado):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text("").FontColor("#FFFFFF");
                            table.Cell().Background("#2C3E50").Padding(5).Text($"L. {datos.Sum(d => d.AhorroMensual):N2}").FontColor("#FFFFFF").Bold().AlignRight();
                            table.Cell().Background("#2C3E50").Padding(5).Text($"{datos.Count(d => d.PorcentajeCompletado >= 100)} completadas").FontColor("#FFFFFF").Bold().AlignCenter();
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