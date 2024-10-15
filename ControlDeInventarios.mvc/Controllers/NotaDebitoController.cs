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
    public class NotaDebitoDTO
    {
        public int PK_codigo { get; set; }
        public string numero { get; set; }
        public DateTime? fecha_hora { get; set; }
        public decimal total { get; set; }
        public string observaciones { get; set; }
        public int anulada { get; set; }
        public DateTime? fecha_anulacion { get; set; }
        public string numero_factura { get; set; }
        public int id_factura { get; set; }
    }
    [AuthAttribute]
    public class NotaDebitoController : Controller
    {
        contexto db = new contexto();
        // GET: NotaDebito
        public ActionResult Index()
        {
            var sqlRaw = "select nota_debito.*,facturacion.numero as numero_factura,facturacion.PK_codigo as id_factura from nota_debito inner join facturacion on facturacion.PK_codigo = nota_debito.FK_facturacion";
            //resut without DBset
            var sqlResult = db.Database.SqlQuery<NotaDebitoDTO>(sqlRaw).ToList();           
         
            return View(sqlResult);
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
                var lastNota = db.notas_debito.OrderByDescending(x => x.PK_codigo).FirstOrDefault();
                var nuevoNumero = "ND1";
                if (lastNota != null)
                {
                    nuevoNumero = "ND" + (lastNota.PK_codigo + 1).ToString();
                }
                _nota.numero =nuevoNumero;
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

        public ActionResult Anular(int id)
        {
            var _nota = db.notas_debito.Where(x => x.PK_codigo == id).FirstOrDefault();

            //if nota not found return json response
            if (_nota == null)
            {
                //return http code 403 json response
                
                return Json(new { success = false, message = "Nota de débito no encontrada" });
            }

            //if nota is already anulated return json response
            if (_nota.anulada==1)
            {
                return Json(new { success = false, message = "Nota de débito ya fue anulada" });
            }
            _nota.fecha_anulacion= DateTime.Now;
            _nota.anulada = 1;
            db.SaveChanges();
            //return json response
            return Json(new { success = true, message = "Nota de débito anulada correctamente" });
        }
    }
}