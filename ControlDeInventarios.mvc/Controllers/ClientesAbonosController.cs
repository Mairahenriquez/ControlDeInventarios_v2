using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;
using ControlDeInventarios.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class ClientesAbonosController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: ClientesAbonos
        public ActionResult Index()
        {
            var _registro = db.vw_clientes_abonos.ToList();
            return View(_registro);
        }

        public ActionResult Details(int id)
        {
            var _registro = db.vw_clientes_abonos.Where(x => x.PK_codigo == id).FirstOrDefault();

            if (_registro != null)
            {
                ViewBag._detalles = db.vw_clientes_abonos_facturas.Where(x => x.FK_abono == id).ToList();
                return View(_registro);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            ViewBag._forma_pago = db.formas_pagos.OrderBy(x => x.PK_codigo).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult Create(vw_clientes_abonos value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Creación de variables y asignación de valores
                        int codigo = 0;

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
                                cmd.Parameters.Add("@ingreso_manual", SqlDbType.Bit).Value = 1;
                                cmd.Parameters.Add("@FK_factura", SqlDbType.Int).Value = (object)value.FK_factura ?? DBNull.Value;
                                cmd.Parameters.Add("@FK_forma_pago", SqlDbType.Int).Value = value.FK_forma_pago;
                                cmd.Parameters.Add("@FK_usuario", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@FK_cuenta_corriente", SqlDbType.Int).Value = value.FK_cuenta_corriente;

                                SqlParameter RETURN_VALUE = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                                RETURN_VALUE.Direction = ParameterDirection.ReturnValue;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                codigo = (int)RETURN_VALUE.Value;
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Abono agregado: {codigo} - {value.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Redirecciona a la vista Details.
                        return RedirectToAction("Details/" + codigo);
                    }
                }
                //Actualiza la vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesAbonosController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _registro = db.vw_clientes_abonos.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_registro);

        }

        [HttpPost]
        public ActionResult Edit(vw_clientes_abonos value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Se actualiza el registro.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_clientes_abonos_editar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = value.PK_codigo;
                                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = value.fecha;
                                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = value.monto;
                                cmd.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = (object)value.referencia ?? DBNull.Value;
                                cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)value.observaciones ?? DBNull.Value;
                                cmd.Parameters.Add("@FK_forma_pago", SqlDbType.Int).Value = value.FK_forma_pago;
                                cmd.Parameters.Add("@FK_cuenta_corriente", SqlDbType.Int).Value = value.FK_cuenta_corriente;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Abono actualizado: {value.PK_codigo} - {value.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return RedirectToAction("Details/" + value.PK_codigo);
                    }
                }
                //Actualiza a vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesAbonosController :: Edit() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult _Modal_Facturas(int id)
        {
            //Busqueda de registro.
            var _modelo = db.vw_clientes_abonos.Where(x => x.PK_codigo == id).FirstOrDefault();
            var _modelo_detalle = db.vw_clientes_abonos_facturas.Where(x => x.FK_abono == id).Select(x => x.FK_factura).ToList();
            ViewBag._facturas = db.vw_facturacion.Where(x => !_modelo_detalle.Contains(x.PK_codigo) && x.saldo > 0).ToList();

            //Devuelve la vista parcial.
            return PartialView(_modelo);
        }

        public ActionResult Insert(vw_clientes_abonos_facturas value)
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
                        var _factura = db.vw_facturacion.Where(x => x.PK_codigo == value.FK_factura).FirstOrDefault();

                        //Guarda el registro en la base de datos.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_clientes_abonos_facturas_agregar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = _factura.saldo;
                                cmd.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = (object)_factura.concepto ?? DBNull.Value;
                                cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)_factura.observaciones ?? DBNull.Value;
                                cmd.Parameters.Add("@FK_abono", SqlDbType.Int).Value = value.FK_abono;
                                cmd.Parameters.Add("@FK_factura", SqlDbType.Int).Value = value.FK_factura;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura agregada en el Abono: {value.FK_abono} - {value.referencia}.";
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
                var descripcion = $"ClientesAbonosController :: Insert() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return Json(value);
            }
        }

        public ActionResult _Modal_Editar_Monto(int id, int codigo)
        {
            //Busqueda de registro.
            var _modelo = db.vw_clientes_abonos_facturas.Where(x => x.PK_codigo == codigo && x.FK_abono == id).FirstOrDefault();

            //Devuelve la vista parcial.
            return PartialView(_modelo);
        }

        public ActionResult Update(vw_clientes_abonos_facturas value)
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
                        var _registro = db.vw_clientes_abonos_facturas.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Guarda el registro en la base de datos.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_clientes_abonos_facturas_editar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = value.PK_codigo;
                                cmd.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = (object)_registro.referencia ?? DBNull.Value;
                                cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)_registro.observaciones ?? DBNull.Value;
                                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = value.monto;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Item actualizado en el Abono: {value.FK_abono} - {value.referencia}.";
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
                var descripcion = $"ClientesAbonosController :: Update() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return Json(value);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                //Buscar registro.
                var _factura = db.vw_clientes_abonos_facturas.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Se valida que el modelo no sea nulo.
                if (_factura != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Guarda el registro en la base de datos.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_clientes_abonos_facturas_eliminar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = _factura.PK_codigo;
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura eliminada en el Abono: {_factura.FK_abono} - {_factura.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Redirecciona a la vista Details.
                        return RedirectToAction("Details/" + _factura.FK_abono);
                    }
                }
                //Actualiza la vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesAbonosController :: Delete() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Procesar(int id)
        {
            try
            {
                //Buscar registro.
                var _registro = db.vw_clientes_abonos.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_registro != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Buscar registro.
                        var _registro_detalle = db.vw_clientes_abonos_facturas.Where(x => x.FK_abono == id).ToList();

                        if (_registro.monto == _registro_detalle.Sum(x => x.monto))
                        {
                            //Guarda el registro en la base de datos.
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                            {
                                using (SqlCommand cmd = new SqlCommand("clientes_abonos_procesar", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = _registro.PK_codigo;

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Abono procesado: {_registro.PK_codigo} - {_registro.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_registro);
                    }
                }
                //Actualizar vista.
                return Json(_registro);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesAbonosController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

    }
}