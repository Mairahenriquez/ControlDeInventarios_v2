using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class KardexVentasController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: KardexVentas
        public ActionResult Index()
        {
            var _registro = db.vw_inventarios_kardex_ventas.OrderByDescending(x => x.existencia_fisica).ToList();
            return View(_registro);
        }
    }
}