using ControlDeInventarios.mvc.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class DevolucionesComprasController : Controller
    {
        // GET: DevolucionesCompras
        public ActionResult Index()
        {
            return View();
        }
    }
}