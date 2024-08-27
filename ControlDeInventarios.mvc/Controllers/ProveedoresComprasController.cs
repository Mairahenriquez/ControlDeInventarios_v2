using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class ProveedoresComprasController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: ProveedoresCompras
        public ActionResult Index()
        {
            var _compras = db.vw_proveedores_compras.ToList();
            return View(_compras);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _compra = db.vw_proveedores_compras.Where(x => x.PK_codigo == id).FirstOrDefault();

            //Validar que sea diferente de null.
            if (_compra != null)
            {
                ViewBag._detalle = db.vw_proveedores_compras_detalle.Where(x => x.FK_compra == id).ToList();
                ViewBag._movimientos = db.vw_inventarios_movimientos.Where(x => x.FK_compra == id).ToList();
                ViewBag._partidas = db.vw_contabilidad_partidas.Where(x => x.FK_compra == id).ToList();
                return View(_compra);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Procesar(int id)
        {
            try
            {
                //Buscar registro.
                var _compra = db.proveedores_compras.Where(x => x.PK_codigo == id).FirstOrDefault();
                var _detalle = db.vw_proveedores_compras_detalle.Where(x => x.FK_compra == id).ToList();

                //Validar que el modelo no sea null.
                if (_compra != null && _detalle.Sum(x => x.total) > 0)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Ejecución de procedimiento.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_procesar_factura_compras", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = id;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura de compra procesada: {_compra.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_compra);
                    }
                }
                //Actualizar vista.
                return Json(_compra);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ProveedoresComprasController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

        public ActionResult Anular(int id)
        {
            try
            {
                //Buscar registro.
                var _compra = db.proveedores_compras.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_compra != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {

                        //Asignar valor.
                        _compra.FK_estado = 3;

                        //Actualizar registro.
                        db.SaveChanges();

                        //Buscar registro.
                        var _orden = db.proveedores_ordenes_compras.Where(x => x.PK_codigo == _compra.FK_orden_compra).FirstOrDefault();

                        if (_orden != null)
                        {
                            //Asignar valor.
                            _orden.FK_estado = 3;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura de compra procesada: {_compra.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_compra);
                    }
                }
                //Actualizar vista.
                return Json(_compra);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ProveedoresComprasController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

    }
}