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

    public class DevolucionCompraController : Controller
    {
        contexto db = new contexto();
        // GET: DevolucionCompra
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}