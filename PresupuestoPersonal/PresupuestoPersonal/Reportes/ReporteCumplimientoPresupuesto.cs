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

using Color = ScottPlot.Color;

namespace PresupuestoPersonal.Reportes
{
    public class FilaCumplimientoPresupuesto
    {
        public string NombreCategoria { get; set; } = string.Empty;
        public string NombreSubcategoria { get; set; } = string.Empty;
        public decimal MontoPresupuestado { get; set; }
        public decimal MontoEjecutado { get; set; }
        public decimal Diferencia { get; set; }
        public decimal PorcentajeEjecucion { get; set; }
    }

    public class ReporteCumplimientoPresupuesto
    {
        private static readonly string[] Meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

        public static List<FilaCumplimientoPresupuesto> ObtenerDatos(int idUsuario, int idPresupuesto, short anio, byte mes)
        {
            List<FilaCumplimientoPresupuesto> lista = new List<FilaCumplimientoPresupuesto>();
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("sp_reporte_cumplimiento_presupuesto", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
                    cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);
                    cmd.Parameters.AddWithValue("@p_anio", anio);
                    cmd.Parameters.AddWithValue("@p_mes", mes);

                    if (conexion.State != ConnectionState.Open) conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new FilaCumplimientoPresupuesto
                            {
                                NombreCategoria = reader.GetString(reader.GetOrdinal("nombre_categoria")),
                                NombreSubcategoria = reader.GetString(reader.GetOrdinal("nombre_subcategoria")),
                                MontoPresupuestado = reader.GetDecimal(reader.GetOrdinal("monto_presupuestado")),
                                MontoEjecutado = reader.GetDecimal(reader.GetOrdinal("monto_ejecutado")),
                                Diferencia = reader.GetDecimal(reader.GetOrdinal("diferencia")),
                                PorcentajeEjecucion = reader.GetDecimal(reader.GetOrdinal("porcentaje_ejecucion"))
                            });
                        }
                    }
                }
            }
            return lista;
        }

        private static string GenerarGrafico(List<FilaCumplimientoPresupuesto> datos)
        {
            string rutaImagen = Path.Combine(Path.GetTempPath(), $"reporte3_{Guid.NewGuid()}.png");
            var myPlot = new ScottPlot.Plot();

            var barras = new List<ScottPlot.Bar>();

            for (int i = 0; i < datos.Count; i++)
            {
                barras.Add(new ScottPlot.Bar()
                {
                    Position = i,
                    Value = (double)datos[i].MontoPresupuestado,
                    FillColor = ScottPlot.Color.FromHex("#3498DB")
                });

                barras.Add(new ScottPlot.Bar()
                {
                    Position = i + 0.3,
                    Value = (double)datos[i].MontoEjecutado,
                    FillColor = ScottPlot.Color.FromHex("#E74C3C")
                });
            }

            myPlot.Add.Bars(barras);

            var tickList = datos.Select((f, i) => new Tick(i + 0.15, f.NombreSubcategoria)).ToArray();
            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(tickList);

            myPlot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            myPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
            myPlot.Axes.Bottom.MinimumSize = 100f; 

            myPlot.Title("Análisis de Presupuesto vs Real");
            myPlot.YLabel("Lempiras (L.)");

            myPlot.SavePng(rutaImagen, 850, 450);
            return rutaImagen;
        }

        public static void Generar(int idUsuario, string nombreUsuario, int idPresupuesto, short anio, byte mes)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            if (!Directory.Exists(carpetaReportes)) Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = LicenseType.Community;

            List<FilaCumplimientoPresupuesto> datos = ObtenerDatos(idUsuario, idPresupuesto, anio, mes);
            string rutaGrafico = GenerarGrafico(datos);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte3_Cumplimiento_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            Document.Create(container => {
                container.Page(page => {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);

                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(QuestPDF.Helpers.Fonts.Verdana));

                    page.Header().Column(col => {
                        col.Item().Text("GESTOR DE PRESUPUESTOS PERSONALES").FontSize(16).Bold().AlignCenter();
                        col.Item().Text($"Análisis de Cumplimiento: {Meses[mes]} {anio}").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario}").FontSize(10).AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1);
                    });

                    page.Content().PaddingTop(10).Column(col => {
                        col.Item().AlignCenter().Image(rutaGrafico);
                        col.Item().PaddingTop(10).Table(table => {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn(3); columns.RelativeColumn(3); columns.RelativeColumn(2);
                                columns.RelativeColumn(2); columns.RelativeColumn(2); columns.RelativeColumn(1); columns.RelativeColumn(2);
                            });

                            table.Header(header => {
                                string hBg = "#3d105c"; string hFg = "#ffffff";
                                header.Cell().Background(hBg).Padding(5).Text("Categoría").FontColor(hFg).Bold();
                                header.Cell().Background(hBg).Padding(5).Text("Subcategoría").FontColor(hFg).Bold();
                                header.Cell().Background(hBg).Padding(5).Text("Presup.").FontColor(hFg).Bold().AlignRight();
                                header.Cell().Background(hBg).Padding(5).Text("Ejecut.").FontColor(hFg).Bold().AlignRight();
                                header.Cell().Background(hBg).Padding(5).Text("Dif.").FontColor(hFg).Bold().AlignRight();
                                header.Cell().Background(hBg).Padding(5).Text("%").FontColor(hFg).Bold().AlignRight();
                                header.Cell().Background(hBg).Padding(5).Text("Estado").FontColor(hFg).Bold().AlignCenter();
                            });

                            bool alternar = false;
                            foreach (var f in datos)
                            {
                                string bg = alternar ? "#F2F2F2" : "#FFFFFF";
                                string eColor = f.PorcentajeEjecucion < 80 ? "#27AE60" : f.PorcentajeEjecucion <= 100 ? "#F39C12" : "#E74C3C";
                                string eTexto = f.PorcentajeEjecucion < 80 ? "OK" : f.PorcentajeEjecucion <= 100 ? "ALERTA" : "EXCEDIDO";

                                table.Cell().Background(bg).Padding(5).Text(f.NombreCategoria);
                                table.Cell().Background(bg).Padding(5).Text(f.NombreSubcategoria);
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.MontoPresupuestado:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.MontoEjecutado:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.Diferencia:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"{f.PorcentajeEjecucion:N1}%").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text(eTexto).FontColor(eColor).Bold().AlignCenter();
                                alternar = !alternar;
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            }).GeneratePdf(rutaPDF);

            if (File.Exists(rutaGrafico)) File.Delete(rutaGrafico);
            Console.WriteLine($"\nReporte generado: {rutaPDF}");
        }
    }
}