using ControlDeInventarios.entities;
using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data;

namespace ControlDeInventarios.mvc.Controllers
{
   

    [AuthAttribute]
    public class DevolucionesVentasController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();

        // GET: DevolucionesVentas
        public ActionResult Index()
        {
            var devoluciones = db.vw_devolucion_ventas.ToList();
            return View(devoluciones);
        }

        public ActionResult Create([Microsoft.AspNetCore.Mvc.FromBody] DevolucionVentaCreateDTO devolucionRequest  )
        {
            if (Request.HttpMethod == "GET")
                return View();
            
            try
            {
                var pedido = db.clientes_pedidos.Where(x => x.PK_codigo == devolucionRequest.pedido).FirstOrDefault();
                if (pedido == null)
                    return Json(new { success = false, error = "Venta no encontrada" }, JsonRequestBehavior.AllowGet);
            
                var detalles = db.clientes_pedidos_detalle.Where(x => x.FK_pedido == devolucionRequest.pedido).ToList();

                if (detalles.Count == 0)
                    return Json(new { success = false, error = "Venta no tiene detalles" }, JsonRequestBehavior.AllowGet);

                //Validar que el modelo no sea null.
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {

                    var devolucion = new devolucion_ventas();
                    devolucion.FK_clientes_pedidos = devolucionRequest.pedido;
                    devolucion.fecha = DateTime.Now;
                    devolucion.observaciones = devolucionRequest.comentario;
                    db.devolucion_ventas.Add(devolucion);

                    db.SaveChanges();

                    foreach (var detalle in detalles)
                    {
                        var devolucionDetalle = new devolucion_venta_detalle();
                        devolucionDetalle.FK_pedido_detalle = detalle.PK_codigo;
                        devolucionDetalle.FK_devolucion_ventas = devolucion.PK_codigo;
                        db.devolucion_venta_detalle.Add(devolucionDetalle);
                    }
                    db.SaveChanges();

                    //Ejecución de procedimiento.
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_clientes_devolucion_ventas", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = pedido.PK_codigo;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    dbContextTransaction.Commit();
                }
                  
                //Actualiza la vista.
                return Json(new { success = true, message = "Devolución de Ventas creada correctamente", id = 0 });
            }
            catch (Exception e)
            {
                //Actualiza la página.
                return new JsonResult { Data = new { result = 0, error = "No se puede procesar la devolución. " + e.Message} };
            }
            
        }

        [Route("SearchVenta/{id}")]
        public ActionResult SearchVenta(int id)
        {
            var existsDevolucion = db.devolucion_ventas.Where(x => x.FK_clientes_pedidos == id).FirstOrDefault();
            if (existsDevolucion != null)
            {
                return Json(new { success = false, error = "Ya existe una devolución para esta pedido" }, JsonRequestBehavior.AllowGet);
            }
            var pedido = db.vw_clientes_pedidos.Where(x => x.PK_codigo == id).FirstOrDefault();
            var detalles = db.vw_clientes_pedidos_detalle.Where(x => x.FK_pedido == id).ToList();

            if (pedido == null)
            {

                return Json(new { success = false, error = "Venta no encontrada" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, pedido, detalles, fecha_pedido = pedido.fecha.ToString("dd/MM/yyyy") }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            var devolucion = db.devolucion_ventas.Where(x => x.PK_codigo == id).FirstOrDefault();
            var pedido = db.clientes_pedidos.Where(x => x.PK_codigo == devolucion.FK_clientes_pedidos).FirstOrDefault();

            var sqlRaw = $"SELECT *,descripcion as producto FROM devolucion_venta_detalle dcd inner join clientes_pedidos_detalle pcd on pcd.PK_codigo = dcd.FK_pedido_detalle  WHERE FK_devolucion_ventas = {devolucion.PK_codigo}";
            var detalles = db.Database.SqlQuery<DevolucionVentasDetalleDTO>(sqlRaw).ToList();

            ViewBag.devolucion = devolucion;
            ViewBag.detalles = detalles;
            ViewBag.pedido = pedido;
            return View();
        }
    }

    public class DevolucionVentaCreateDTO
    {
        public int pedido { get; set; }
        public string comentario { get; set; }

    }
    public class DevolucionVentasDetalleDTO
    {
        public int PK_codigo { get; set; }
        public int FK_devolucion_ventas { get; set; }
        public int FK_pedido_detalle { get; set; }

        public string producto { get; set; }
        public decimal cantidad { get; set; }
        public decimal iva { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
    }
}