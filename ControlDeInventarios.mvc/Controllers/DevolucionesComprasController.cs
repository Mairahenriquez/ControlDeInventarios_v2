using ControlDeInventarios.entities;
using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

using System.Web.Mvc;

namespace ControlDeInventarios.mvc.Controllers
{
    public class devolucion_compra_dto
    {
      
        public int PK_codigo { get; set; }
        public int FK_proveedores_compras { get; set; }

        public DateTime fecha { get; set; }

        public string observaciones { get; set; }
        public decimal total { get; set; }

    }
    public class DevolucionCompraCreateDTO
    {
        public int compra { get; set; }
        public string comentario { get; set; }
      
    }
    public class DevolucionComprasDetalleDTO
    {
        public int PK_codigo { get; set; }
        public int FK_devolucion_compra { get; set; }
        public int FK_compra_detalle { get; set; }

        public string producto { get; set; }
        public decimal cantidad { get; set; }
        public decimal iva { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
    }

    [AuthAttribute]
    public class DevolucionesComprasController : Controller
    {
        contexto db = new contexto();
        // GET: DevolucionesCompras
        public ActionResult Index()
        {
            var devolucionesSqlRaw = "select devolucion_compras.*,proveedores_compras.total as total from devolucion_compras inner join proveedores_compras on proveedores_compras.PK_codigo = devolucion_compras.FK_proveedores_compras "; 
            var devoluciones = db.Database.SqlQuery<devolucion_compra_dto>(devolucionesSqlRaw).ToList();        
            return View(devoluciones);
        }
        public ActionResult Create([Microsoft.AspNetCore.Mvc.FromBody] DevolucionCompraCreateDTO devolucionRequest  )
        {
            if (Request.HttpMethod == "GET")
                return View();
            
            var compra = db.proveedores_compras.Where(x => x.PK_codigo == devolucionRequest.compra).FirstOrDefault();
            if (compra == null)
                return Json(new { success = false, error = "Compra no encontrada" }, JsonRequestBehavior.AllowGet);
            
            var detalles = db.proveedores_compras_detalles.Where(x => x.FK_compra == devolucionRequest.compra).ToList();

            if (detalles.Count == 0)
                return Json(new { success = false, error = "Compra no tiene detalles" }, JsonRequestBehavior.AllowGet);
           
            using (var dbContextTransaction = db.Database.BeginTransaction()) {

                var devolucion = new devolucion_compra();
                devolucion.FK_proveedores_compras = devolucionRequest.compra;
                devolucion.fecha = DateTime.Now;
                devolucion.observaciones = devolucionRequest.comentario;
                db.devolucion_compra.Add(devolucion);

                db.SaveChanges();

                foreach (var detalle in detalles)
                {
                    var devolucionDetalle = new devolucion_compra_detalle();
                    devolucionDetalle.FK_compra_detalle = detalle.PK_codigo;
                    devolucionDetalle.FK_devolucion_compra = devolucion.PK_codigo;
                    db.devolucion_compra_detalles.Add(devolucionDetalle);
                }
                db.SaveChanges();

                dbContextTransaction.Commit();
            }

            return Json(new { success = true, message = "Nota de débito creada correctamente", id = 0 });
            
        }

        [Route("SearchCompra/{id}")]
        public ActionResult SearchCompra(int id)
        {
            var existsDevolucion = db.devolucion_compra.Where(x => x.FK_proveedores_compras == id).FirstOrDefault();
            if (existsDevolucion != null)
            {
                return Json(new { success = false, error = "Ya existe una devolución para esta compra" }, JsonRequestBehavior.AllowGet);
            }
            var compra = db.vw_proveedores_compras.Where(x => x.PK_codigo == id).FirstOrDefault();
            var detalles = db.vw_proveedores_compras_detalle.Where(x => x.FK_compra == id).ToList();

            if (compra == null)
            {

                return Json(new { success = false, error = "Compra no encontrada" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, compra, detalles, fecha_compra = compra.fecha.ToString("dd/MM/yyyy") }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            var devolucion = db.devolucion_compra.Where(x => x.PK_codigo == id).FirstOrDefault();
            var compra = db.proveedores_compras.Where(x => x.PK_codigo == devolucion.FK_proveedores_compras).FirstOrDefault();
            // var detalles = db.devolucion_compra_detalles.Where(x => x.FK_devolucion_compra == id).ToList();

            var sqlRaw = $"SELECT *,descripcion as producto FROM devolucion_compra_detalle dcd inner join proveedores_compras_detalle pcd on pcd.PK_codigo = dcd.FK_compra_detalle  WHERE FK_devolucion_compra = {devolucion.PK_codigo}";
            var detalles = db.Database.SqlQuery<DevolucionComprasDetalleDTO>(sqlRaw).ToList();

            ViewBag.devolucion = devolucion;
            ViewBag.detalles = detalles;
            ViewBag.compra = compra;
            return View();
        }
    }
}