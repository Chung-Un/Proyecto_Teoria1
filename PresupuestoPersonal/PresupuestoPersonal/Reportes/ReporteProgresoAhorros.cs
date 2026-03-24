using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace PresupuestoPersonal.Reportes
{
    public class FilaProgresoAhorro
    {
        public string NombreMeta { get; set; } = string.Empty;
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
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("sp_reporte_progreso_ahorros", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
                    if (conexion.State != ConnectionState.Open) conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
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
                    }
                }
            }
            return lista;
        }

        private static string GenerarGrafico(List<FilaProgresoAhorro> datos)
        {
            string rutaImagen = Path.Combine(Path.GetTempPath(), $"reporte6_{Guid.NewGuid()}.png");
            var myPlot = new ScottPlot.Plot();
            var barras = new List<ScottPlot.Bar>();

            for (int i = 0; i < datos.Count; i++)
            {
                barras.Add(new ScottPlot.Bar()
                {
                    Position = (double)i,
                    Value = (double)datos[i].MontoObjetivoAnual,
                    FillColor = ScottPlot.Color.FromHex("#BDC3C7")
                });

                barras.Add(new ScottPlot.Bar()
                {
                    Position = (double)i + 0.3,
                    Value = (double)datos[i].MontoAcumulado,
                    FillColor = ScottPlot.Color.FromHex("#3498DB")
                });
            }

            myPlot.Add.Bars(barras);

            var tickList = new List<Tick>();
            for (int i = 0; i < datos.Count; i++)
            {
                tickList.Add(new Tick((double)i + 0.15, datos[i].NombreMeta));
            }
            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(tickList.ToArray());

            myPlot.Title("Progreso de Metas");
            myPlot.SavePng(rutaImagen, 700, 400);
            return rutaImagen;
        }

        public static void Generar(int idUsuario, string nombreUsuario)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            if (!Directory.Exists(carpetaReportes)) Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            List<FilaProgresoAhorro> datos = ObtenerDatos(idUsuario);
            string rutaGrafico = GenerarGrafico(datos);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte6_Ahorros_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre); // Corregido a float para evitar CS1503
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(QuestPDF.Helpers.Fonts.Verdana));

                    page.Header().Text("REPORTE DE METAS DE AHORRO").FontSize(16).Bold().AlignCenter();

                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Item().AlignCenter().Image(rutaGrafico);
                        col.Item().PaddingTop(15).Table(table =>
                        {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn(3); columns.RelativeColumn(2); columns.RelativeColumn(2);
                            });

                            foreach (var f in datos)
                            {
                                table.Cell().Text(f.NombreMeta);
                                table.Cell().AlignRight().Text($"L. {f.MontoObjetivoAnual:N2}");
                                table.Cell().AlignRight().Text($"{f.PorcentajeCompletado:N1}%");
                            }
                        });
                    });
                });
            }).GeneratePdf(rutaPDF);

            if (File.Exists(rutaGrafico)) File.Delete(rutaGrafico);
            Console.WriteLine("Reporte generado.");
        }
    }
}