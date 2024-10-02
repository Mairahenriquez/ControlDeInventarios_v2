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
using ControlDeInventarios.entities;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class ClientesFacturacionController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: ClientesFacturacion
        public ActionResult Index()
        {
            var _facturas = db.vw_facturacion.ToList();
            return View(_facturas);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _factura = db.vw_facturacion.Where(x => x.PK_codigo == id).FirstOrDefault();

            //Validar que sea diferente de null.
            if (_factura != null)
            {
                ViewBag._detalle = db.vw_facturacion_detalle.Where(x => x.FK_factura == id).ToList();
                ViewBag._movimientos = db.vw_inventarios_movimientos.Where(x => x.FK_factura == id).ToList();
                ViewBag._partidas = db.vw_contabilidad_partidas.Where(x => x.FK_factura == id).ToList();
                ViewBag._abonos = db.vw_clientes_abonos.Where(x => x.FK_factura == id).ToList();
                return View(_factura);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Procesar(int id, int FK_bodega)
        {
            try
            {
                //Buscar registro.
                var _factura = db.facturacion.Where(x => x.PK_codigo == id).FirstOrDefault();
                var _detalle = db.vw_facturacion_detalle.Where(x => x.FK_factura == id).ToList();

                //Validar que el modelo no sea null.
                if (_factura != null && _detalle.Sum(x => x.total) > 0)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valor.
                        _factura.FK_bodega = FK_bodega;

                        //Actualizar registro.
                        db.SaveChanges();

                        //Ejecución de procedimiento.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_procesar_factura_ventas", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = id;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura procesada: {_factura.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_factura);
                    }
                }
                //Actualizar vista.
                return Json(_factura);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesFacturacionController :: Procesar() :: {e.Message}.";
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
                var _factura = db.facturacion.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_factura != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {

                        //Asignar valor.
                        _factura.FK_estado = 3;

                        //Actualizar registro.
                        db.SaveChanges();

                        //Buscar registro.
                        var _pedido = db.clientes_pedidos.Where(x => x.PK_codigo == _factura.FK_pedido).FirstOrDefault();

                        if (_pedido != null)
                        {
                            //Asignar valor.
                            _pedido.FK_estado = 3;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura anulada: {_factura.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_factura);
                    }
                }
                //Actualizar vista.
                return Json(_factura);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesFacturacionController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

        public ActionResult _Modal_Abonos(int id)
        {
            //Busqueda de registro.
            var _modelo = db.vw_facturacion.Where(x => x.PK_codigo == id).FirstOrDefault();
            ViewBag._forma_pago = db.formas_pagos.OrderBy(x => x.PK_codigo).FirstOrDefault();

            //Devuelve la vista parcial.
            return PartialView(_modelo);
        }

        public ActionResult InsertAbono(vw_clientes_abonos value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Buscar registro.
                        var codigo = 0;

                        //Guarda el registro en la base de datos.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_clientes_abonos_agregar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = value.fecha;
                                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = value.monto;
                                cmd.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = value.referencia;
                                cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)value.observaciones ?? DBNull.Value;
                                cmd.Parameters.Add("@ingreso_manual", SqlDbType.Bit).Value = 0;
                                cmd.Parameters.Add("@FK_factura", SqlDbType.Int).Value = (object)value.FK_factura ?? DBNull.Value;
                                cmd.Parameters.Add("@FK_forma_pago", SqlDbType.Int).Value = value.FK_forma_pago;
                                cmd.Parameters.Add("@FK_usuario", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@FK_cuenta_corriente", SqlDbType.Int).Value = (object)value.FK_cuenta_corriente ?? DBNull.Value;

                                SqlParameter RETURN_VALUE = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                                RETURN_VALUE.Direction = ParameterDirection.ReturnValue;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                codigo = (int)RETURN_VALUE.Value;
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Abono agregadoo: {codigo} - {value.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Redirecciona a la vista Details.
                        return Json(value);
                    }
                }
                //Actualiza la vista.
                return Json(value);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesFacturacion :: InsertAbonos() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return Json(value);
            }
        }

        public ActionResult DeleteAbono(int id)
        {
            try
            {
                //Buscar registro.
                var _registro = db.vw_clientes_abonos.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Se valida que el modelo no sea nulo.
                if (_registro != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Guarda el registro en la base de datos.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_clientes_abonos_eliminar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = _registro.PK_codigo;
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura eliminada en el Abono: {_registro.PK_codigo} - {_registro.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Redirecciona a la vista Details.
                        return RedirectToAction("Details/" + _registro.FK_factura);
                    }
                }
                //Actualiza la vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesAbonosController :: Eliminar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

    }
}