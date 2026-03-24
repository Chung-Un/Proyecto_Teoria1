using Microsoft.Data.SqlClient;
using PresupuestoPersonal.DataAccess;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PresupuestoPersonal.Reportes
{
    public class FilaCumplimientoPresupuesto
    {
        public string NombreCategoria { get; set; }
        public string NombreSubcategoria { get; set; }
        public decimal MontoPresupuestado { get; set; }
        public decimal MontoEjecutado { get; set; }
        public decimal Diferencia { get; set; }
        public decimal PorcentajeEjecucion { get; set; }
    }

    public class ReporteCumplimientoPresupuesto
    {
        public static List<FilaCumplimientoPresupuesto> ObtenerDatos(int idUsuario, int idPresupuesto, short anio, byte mes)
        {
            List<FilaCumplimientoPresupuesto> lista = new List<FilaCumplimientoPresupuesto>();

            using SqlConnection conexion = Conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_reporte_cumplimiento_presupuesto", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id_usuario", idUsuario);
            cmd.Parameters.AddWithValue("@p_id_presupuesto", idPresupuesto);
            cmd.Parameters.AddWithValue("@p_anio", anio);
            cmd.Parameters.AddWithValue("@p_mes", mes);

            using SqlDataReader reader = cmd.ExecuteReader();
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
            return lista;
        }

        public static void Generar(int idUsuario, string nombreUsuario, int idPresupuesto, short anio, byte mes)
        {
            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);

            QuestPDF.Settings.License = LicenseType.Community;

            List<FilaCumplimientoPresupuesto> datos = ObtenerDatos(idUsuario, idPresupuesto, anio, mes);
            string rutaPDF = Path.Combine(carpetaReportes, $"Reporte3_CumplimientoPresupuesto_{nombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            string[] meses = { " ", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

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
                        col.Item().Text("Reporte 3: Analisis de Cumplimiento de Presupuesto por Categoria y Subcategoria").FontSize(12).AlignCenter();
                        col.Item().Text($"Usuario: {nombreUsuario} | Periodo: {meses[mes]} {anio}").FontSize(10).AlignCenter();
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
                                columns.RelativeColumn(3); 
                                columns.RelativeColumn(3);  
                                columns.RelativeColumn(2); 
                                columns.RelativeColumn(2); 
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#3d105c").Padding(5).Text("Categoria").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Subcategoria").FontColor("#ffffff").Bold();
                                header.Cell().Background("#3d105c").Padding(5).Text("Presupuestado").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Ejecutado").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Diferencia").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("%").FontColor("#ffffff").Bold().AlignRight();
                                header.Cell().Background("#3d105c").Padding(5).Text("Estado").FontColor("#ffffff").Bold().AlignCenter();
                            });

                            bool fila = false;
                            foreach (FilaCumplimientoPresupuesto f in datos)
                            {
                                string bg = fila ? "#F2F2F2" : "#FFFFFF";

                                string estadoColor;
                                string estadoTexto;
                                if (f.PorcentajeEjecucion < 80)
                                {
                                    estadoColor = "#27AE60";
                                    estadoTexto = "OK";
                                }
                                else if (f.PorcentajeEjecucion <= 100)
                                {
                                    estadoColor = "#F39C12";
                                    estadoTexto = "ALERTA";
                                }
                                else
                                {
                                    estadoColor = "#E74C3C";
                                    estadoTexto = "EXCEDIDO";
                                }

                                table.Cell().Background(bg).Padding(5).Text(f.NombreCategoria);
                                table.Cell().Background(bg).Padding(5).Text(f.NombreSubcategoria);
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.MontoPresupuestado:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.MontoEjecutado:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"L. {f.Diferencia:N2}").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text($"{f.PorcentajeEjecucion:N1}%").AlignRight();
                                table.Cell().Background(bg).Padding(5).Text(estadoTexto).FontColor(estadoColor).Bold().AlignCenter();

                                fila = !fila;
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