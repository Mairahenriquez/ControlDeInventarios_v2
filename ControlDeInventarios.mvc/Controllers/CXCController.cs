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

    }
}