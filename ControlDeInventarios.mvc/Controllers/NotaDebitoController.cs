using ControlDeInventarios.entities;
using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDeInventarios.mvc.Controllers
{
    public class CargoDTO
    {
       public string concepto { get; set; }
       public decimal valor { get; set; }
    }
    //cargos array
    public class PostDataDTO
    {
      public CargoDTO[] cargos { get; set; }
      public string comentario { get; set; }
    }

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

        public ActionResult Create(int id, [Microsoft.AspNetCore.Mvc.FromBody] PostDataDTO postData) {

            var _factura = db.facturacion.Where(x => x.PK_codigo == id).FirstOrDefault();
            ViewBag.factura = _factura;

            //if method is post
            if (Request.HttpMethod == "POST")
            {

                var totalCargos = postData.cargos.Sum(x => x.valor);

                var _nota = new nota_debito();
                _nota.FK_facturacion = id;
                _nota.fecha_hora = DateTime.Now;
                //sum one to the last number of nota_debito table
                _nota.numero = "ND" + (db.notas_debito.OrderByDescending(x => x.PK_codigo).FirstOrDefault().PK_codigo + 1).ToString();
                _nota.total = totalCargos;
                _nota.observaciones = postData.comentario;
                db.notas_debito.Add(_nota);

                db.SaveChanges();
                //create detalles form cargos
                foreach (var item in postData.cargos)
                {
                    var _detalle = new nota_debito_detalle();
                    _detalle.FK_nota_debito = _nota.PK_codigo;
                    _detalle.concepto = item.concepto;
                    _detalle.total = item.valor;
                    db.nota_debito_detalles.Add(_detalle);
                }
                db.SaveChanges();
                var createdId = _nota.PK_codigo;
                //return with json params
                return Json(new { success = true, message = "Nota de débito creada correctamente" ,id=createdId});
            }
            return View();
        }
        public ActionResult Details(int id)
        {

            var _nota = db.notas_debito.Where(x => x.PK_codigo == id).FirstOrDefault();
            var _detalle = db.nota_debito_detalles.Where(x => x.FK_nota_debito == id).ToList();
            ViewBag.nota = _nota;
            ViewBag.detalle = _detalle;
            return View();
        } 
    }
}