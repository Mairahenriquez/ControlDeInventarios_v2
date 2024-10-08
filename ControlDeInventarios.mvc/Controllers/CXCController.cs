using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Web;
using System.Web.Mvc;
using Document = QuestPDF.Fluent.Document;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class CXCController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: CXC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CorteCaja()
        {
            return View();
        }

        public ActionResult _GenerarCorte(DateTime? fecha)
        {
            //Validar fecha.
            fecha = fecha == null ? DateTime.Now : fecha;

            //LLenar listas.
            object[] parametros1 = {
                new SqlParameter("@fecha", (object)fecha ?? DBNull.Value)
                };
            var corte_caja = db.reporte_corte_caja.SqlQuery("sp_reporte_corte_caja  @fecha", parametros1).OrderBy(x => x.PK_codigo).ToList();

            //Devuelve la vista parcial;
            return PartialView(corte_caja);
        }

        public ActionResult GenerarReporteCorteCaja(DateTime fecha)
        {
            //Buscar registro.
            QuestPDF.Settings.License = LicenseType.Community;

            //LLenar listas.
            object[] parametros1 = {
                new SqlParameter("@fecha", (object)fecha ?? DBNull.Value)
                };
            var lista_general = db.reporte_corte_caja.SqlQuery("sp_reporte_corte_caja  @fecha", parametros1).OrderBy(x => x.PK_codigo).ToList();

            //Formato para números negativos.
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            var stream = new MemoryStream();
            Document.Create(document =>
            {
                //Document creation here
                document.Page(page =>
                {
                    page.Margin(70);

                    //ENCABEZADO DEL REPORTE
                    page.Header().Row(row =>
                    {
                        row.Spacing(10);
                        row.RelativeItem().PaddingTop(20).AlignCenter().Column(col =>
                        {
                            //ASIGNAR VALORES.
                            var dia = fecha.Day;
                            //var mes = fecha.ToString("MMMM");
                            var mes = fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                            var anio = fecha.Year;

                            //TABLA 1.0
                            col.Item().PaddingTop(2).BorderColor("#D9D9D9").Table(tabla1 =>
                            {
                                tabla1.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                });
                                tabla1.Header(header =>
                                {

                                });
                                tabla1.Cell().BorderColor("#D9D9D9")
                                        .Padding(0).Text("Control de Inventario InnovaTec".ToUpper()).Bold().AlignCenter().FontFamily("Figtree").FontSize(20);

                                tabla1.Cell().BorderColor("#D9D9D9")
                                        .Padding(0).Text("CIERRE DEL DIA " + dia + " DE " + mes.ToUpper() + " " + anio).Bold().AlignCenter().FontFamily("Figtree").FontSize(14);
                            });

                            //TABLA 1.1
                            col.Item().PaddingTop(30).BorderColor("#D9D9D9").Table(tabla1 =>
                            {
                                tabla1.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(90);
                                    columns.RelativeColumn();
                                });
                                tabla1.Header(header =>
                                {

                                });
                                tabla1.Cell().BorderColor("#D9D9D9")
                                        .Padding(0).Text("PUNTO DE VENTA: ").Bold().AlignLeft().FontFamily("Figtree").FontSize(10);

                                tabla1.Cell().BorderColor("#D9D9D9")
                                        .Padding(0).Text("SAN SALVADOR").AlignLeft().FontFamily("Figtree").FontSize(10);
                            });
                        });
                    });

                    //CUERPO DEL REPORTE.
                    page.Content().PaddingVertical(0).Column(col1 =>
                    {
                        //INICIO DE TABLA 2.
                        col1.Item().PaddingTop(20).Row(row =>
                        {
                            row.Spacing(20);

                            //TABLA 2
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Border(0.5f).BorderColor("#D9D9D9").Table(tabla2 =>
                                {
                                    tabla2.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(100);
                                        columns.RelativeColumn();
                                        columns.ConstantColumn(100);
                                    });
                                    tabla2.Header(header =>
                                    {
                                        header.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9").Background("#D9D9D9")
                                        .Padding(2).Text("TIPO").FontSize(10).AlignCenter().FontFamily("Figtree");

                                        header.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9").Background("#D9D9D9")
                                       .Padding(2).Text("FORMA DE PAGO").AlignRight().FontSize(10).AlignCenter().FontFamily("Figtree");

                                        header.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9").Background("#D9D9D9")
                                       .Padding(2).Text("MONTO").AlignRight().FontSize(10).AlignCenter().FontFamily("Figtree");
                                    });
                                    var cont = 1;
                                    foreach (var item in lista_general)
                                    {
                                        if (item.tipo == "PRODUCTOS")
                                        {
                                            if (cont == 3)
                                            {
                                                tabla2.Cell().BorderRight(0.5f).BorderColor("#D9D9D9")
                                                    .Padding(2).Text(item.tipo).AlignCenter().FontFamily("Figtree").FontSize(10);
                                            }
                                            else
                                            {
                                                tabla2.Cell().BorderRight(0.5f).BorderColor("#D9D9D9")
                                                    .Padding(2).Text("").FontFamily("Figtree").FontSize(10);
                                            }
                                            tabla2.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).Text(item.forma_pago).FontFamily("Figtree").FontSize(10);

                                            tabla2.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).Text(item.total.ToString("C2")).AlignRight().FontFamily("Figtree").FontSize(10);
                                            cont++;
                                        }
                                    }
                                    foreach (var item in lista_general)
                                    {
                                        if (item.tipo == "TOTAL")
                                        {

                                            tabla2.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(1).Text("").FontFamily("Figtree").FontSize(10);

                                            tabla2.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(1).Text("TOTAL").Bold().FontFamily("Figtree").FontSize(10);

                                            tabla2.Cell().BorderRight(0.5f).BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(1).Text(item.total.ToString("C2")).Bold().AlignRight().FontFamily("Figtree").FontSize(10);
                                        }
                                    }
                                });
                            });
                        });

                        //FIRMAS.
                        col1.Item().PaddingTop(100).Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            tabla.Header(header =>
                            {
                                header.Cell()
                                .Padding(2).Text("______________________________").AlignCenter().FontSize(10).AlignCenter();

                                header.Cell()
                                .Padding(2).Text("______________________________").AlignCenter().FontSize(10).AlignCenter();
                            });
                            tabla.Cell().BorderColor("#D9D9D9")
                                    .Padding(2).Text("REVISADO POR:").AlignCenter().FontFamily("Figtree").FontSize(10);

                            tabla.Cell().BorderColor("#D9D9D9")
                                    .Padding(2).Text("AUTORIZADO POR:").AlignCenter().FontFamily("Figtree").FontSize(10);
                        });
                    });
                });
            }).GeneratePdf(stream);
            stream.Position = 0;
            var bytes = stream.ToArray();
            var base64String = Convert.ToBase64String(bytes);
            return Json(new { success = true, pdfBase64 = base64String });
        }

    }
}