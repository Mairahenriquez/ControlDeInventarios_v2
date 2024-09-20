using ControlDeInventarios.entities;
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
    public class NotaCreditoController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: NotasCredito
        public ActionResult Index()
        {
            var _registro = db.vw_tesoreria_notas.ToList();
            return View(_registro);
        }

        public ActionResult Details(int id)
        {
            var _registro = db.vw_tesoreria_notas.Where(x => x.PK_codigo == id).FirstOrDefault();

            if (_registro != null)
            {
                ViewBag._detalles = db.vw_tesoreria_notas_facturas.Where(x => x.FK_tesoreria_nota == id).ToList();
                ViewBag._partidas = db.vw_tesoreria_notas_partidas.Where(x => x.FK_tesoreria_nota == id).ToList();
                return View(_registro);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(vw_tesoreria_notas value)
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
                            using (SqlCommand cmd = new SqlCommand("sp_tesoreria_notas_agregar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = (object)value.referencia ?? DBNull.Value;
                                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = value.monto;
                                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = value.fecha;
                                cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = (object)value.descripcion ?? DBNull.Value;
                                cmd.Parameters.Add("@abono", SqlDbType.Bit).Value = true;
                                cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)value.observaciones ?? DBNull.Value;
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
                        var descripcion = $"Nota de Crédito agregada: {codigo} - {value.referencia}.";
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
                var descripcion = $"NotasCreditoController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _registro = db.vw_tesoreria_notas.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_registro);

        }
        [HttpPost]
        public ActionResult Edit(vw_tesoreria_notas value)
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
                            using (SqlCommand cmd = new SqlCommand("sp_tesoreria_notas_editar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = value.PK_codigo;
                                cmd.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = (object)value.referencia ?? DBNull.Value;
                                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = value.monto;
                                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = value.fecha;
                                cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = (object)value.descripcion ?? DBNull.Value;
                                cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)value.observaciones ?? DBNull.Value;
                                cmd.Parameters.Add("@FK_cuenta_corriente", SqlDbType.Int).Value = value.FK_cuenta_corriente;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Nota de Crédito actualizada: {value.PK_codigo} - {value.referencia}.";
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
                var descripcion = $"NotasCreditoController :: Edit() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Desactivar(int id)
        {
            try
            {
                //Buscar registro.
                var _registro = db.proveedores.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_registro != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _registro.FK_estado = 2;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Proveedor inactivo: {_registro.PK_codigo} - {_registro.nombre_comercial}.";
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
                var descripcion = $"NotasCreditoController :: Desactivar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

        public ActionResult Activar(int id)
        {
            try
            {
                //Buscar registro.
                var _registro = db.proveedores.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_registro != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _registro.FK_estado = 1;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Proveedor activo: {_registro.PK_codigo} - {_registro.nombre_comercial}.";
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
                var descripcion = $"ProveedorController :: Activar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

        public ActionResult _Modal_Facturas(int id)
        {
            //Busqueda de registro.
            var _modelo = db.vw_tesoreria_notas.Where(x => x.PK_codigo == id).FirstOrDefault();
            ViewBag._facturas = db.vw_facturacion.Where(x => x.FK_estado == 2 || x.FK_estado == 3).ToList();

            //Devuelve la vista parcial.
            return PartialView(_modelo);
        }

        public ActionResult Insert(vw_tesoreria_notas_facturas value)
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
                            using (SqlCommand cmd = new SqlCommand("sp_tesoreria_notas_facturas_agregar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = (object)_factura.concepto ?? DBNull.Value;
                                cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)_factura.observaciones ?? DBNull.Value;
                                cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = DBNull.Value;
                                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = _factura.total;
                                cmd.Parameters.Add("@FK_tesoreria_nota", SqlDbType.Int).Value = value.FK_tesoreria_nota;
                                cmd.Parameters.Add("@FK_factura", SqlDbType.Int).Value = value.FK_factura;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura agregada en la Nota de Crédito: {value.FK_tesoreria_nota} - {value.referencia}.";
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
                var descripcion = $"NotasCreditoController :: Insert() :: {e.Message}.";
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
                var _factura = db.vw_tesoreria_notas_facturas.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Se valida que el modelo no sea nulo.
                if (_factura != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Guarda el registro en la base de datos.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_tesoreria_notas_facturas_eliminar", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = _factura.PK_codigo;
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Factura eliminada en la Nota de Crédito: {_factura.FK_tesoreria_nota} - {_factura.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Redirecciona a la vista Details.
                        return RedirectToAction("Details/" + _factura.FK_tesoreria_nota);
                    }
                }
                //Actualiza la vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"NotasCreditoController :: Delete() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

    }
}