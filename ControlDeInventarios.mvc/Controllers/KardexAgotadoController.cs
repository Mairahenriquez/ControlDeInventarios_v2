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
    public class KardexAgotadoController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: KardexAgotado
        public ActionResult Index()
        {
            var _registro = db.vw_inventarios.Where(x => x.existencia_fisica <= 0).ToList();
            return View(_registro);
        }
    }
}