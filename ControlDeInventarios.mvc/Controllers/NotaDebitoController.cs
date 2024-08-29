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
    public class NotaDebitoController : Controller
    {
        contexto db = new contexto();
        // GET: NotaDebito
        public ActionResult Index()
        {
            var list = db.notas_debito.ToList();
            return View(list);
        }
        
        public ActionResult Create(int id) { 

            var _factura = db.facturacion.Where(x => x.PK_codigo == id).FirstOrDefault();
            ViewBag.factura = _factura;
            return View();
        }
    }
}