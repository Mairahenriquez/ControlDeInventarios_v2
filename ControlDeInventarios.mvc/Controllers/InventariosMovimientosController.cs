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
    public class InventariosMovimientosController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: InventariosMovimientos
        public ActionResult Index()
        {
            var _registro = db.vw_inventarios_movimientos.OrderByDescending(x => x.PK_codigo).ToList();
            return View(_registro);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _registro = db.vw_inventarios_movimientos.Where(x => x.PK_codigo == id).FirstOrDefault();

            //Validar que sea diferente de null.
            if (_registro != null)
            {
                ViewBag._detalle = db.vw_inventarios_movimientos_detalle.Where(x => x.FK_movimiento == id).ToList();
                return View(_registro);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            ViewBag.TipoMovimiento = db.inventarios_movimientos_tipos.Where(x => x.PK_codigo == 3).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult Create(inventarios_movimientos value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    //if (ModelState.IsValid)
                    {
                        //Se asignan valores iniciales.
                        value.fecha_hora = DateTime.Now;
                        value.precio_unitario = 0;
                        value.costo_unitario = 0;
                        value.precio_total = 0;
                        value.costo_total = 0;
                        value.FK_estado = 1;
                        value.FK_usuario = 1;

                        //Guarda el registro en la base de datos.
                        db.inventarios_movimientos.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Movimiento de inventario agregado: {value.PK_codigo} - {value.referencia}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Redirecciona a la vista Details.
                        return RedirectToAction("Details/" + value.PK_codigo);
                    }
                }
                //Actualiza la vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"InventariosMovimientosController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _registro = db.vw_inventarios_movimientos.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_registro);

        }
        [HttpPost]
        public ActionResult Edit(vw_inventarios_movimientos value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Se busca el registro.
                        var _registro = db.inventarios_movimientos.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Se igualan valores.
                        _registro.fecha = value.fecha;
                        _registro.referencia = value.referencia;
                        _registro.observaciones = value.observaciones;
                        _registro.FK_tipo_movimiento = value.FK_tipo_movimiento;
                        _registro.FK_bodega = value.FK_bodega;
                        _registro.FK_bodega_destino = value.FK_bodega_destino;

                        //Se actualiza el registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Movimiento de inventario actualizado: {value.PK_codigo} - {value.referencia}.";
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
                var descripcion = $"InventariosMovimientosController :: Edit() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Insert(inventarios_movimientos_detalle value)
        {
            try
            {
                //Validar que el modelo no sea null.
                if (value != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Buscar registro.
                        var _inventario = db.inventarios.Where(x => x.PK_codigo == value.FK_inventario).FirstOrDefault();
                        var _registro = db.inventarios_movimientos.Where(x => x.PK_codigo == value.FK_movimiento).FirstOrDefault();

                        //Asignar valores.
                        value.descripcion = _inventario.descripcion;
                        value.costo_unitario = _inventario.costo_unitario;
                        value.precio_unitario = (_inventario.precio_unitario == 0) ? _inventario.costo_unitario + (_inventario.costo_unitario * (_inventario.porcentaje_ganacia / 100)) : _inventario.precio_unitario;
                        value.costo_total = _inventario.costo_unitario * value.cantidad;
                        value.precio_total = value.precio_unitario * value.cantidad;
                        value.FK_bodega = _registro.FK_bodega;
                        value.FK_movimiento = _registro.PK_codigo;

                        //Agrega el regstro.
                        db.inventarios_movimientos_detalle.Add(value);
                        db.SaveChanges();

                        //Actualiza el registro de orden de compra.
                        var _detalles = db.inventarios_movimientos_detalle.Where(x => x.FK_movimiento == value.FK_movimiento).ToList();

                        //Si es mayor a cero.
                        if (_detalles.Count() > 0 && _detalles.Sum(x => x.costo_total) > 0)
                        {
                            //Asignar valores.
                            _registro.costo_unitario = _detalles.Sum(x => x.costo_total);
                            _registro.costo_total = _detalles.Sum(x => x.costo_total);
                            _registro.precio_unitario = _detalles.Sum(x => x.precio_total);
                            _registro.precio_total = _detalles.Sum(x => x.precio_total);

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        else
                        {
                            //Asignar valores.
                            _registro.costo_unitario = 0;
                            _registro.costo_total = 0;
                            _registro.precio_unitario = 0;
                            _registro.precio_total = 0;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Detalle de Movimiento de Inventario agregado: {value.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(value);
                    }
                }
                //Actualizar vista.
                return Json(value);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"InventariosMovimientosController :: Insert() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(value);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                //Buscar registro.
                var _detalle = db.inventarios_movimientos_detalle.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_detalle != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Agrega el regstro.
                        db.inventarios_movimientos_detalle.Remove(_detalle);
                        db.SaveChanges();

                        //Actualiza el registro de orden de compra.
                        var _detalles = db.inventarios_movimientos_detalle.Where(x => x.FK_movimiento == _detalle.FK_movimiento).ToList();
                        var _registro = db.inventarios_movimientos.Where(x => x.PK_codigo == _detalle.FK_movimiento).FirstOrDefault();

                        //Si es mayor a cero.
                        if (_detalles.Count() > 0 && _detalles.Sum(x => x.costo_total) > 0 && _registro != null)
                        {
                            //Asignar valores.
                            _registro.costo_unitario = _detalles.Sum(x => x.costo_total);
                            _registro.costo_total = _detalles.Sum(x => x.costo_total);
                            _registro.precio_unitario = _detalles.Sum(x => x.precio_total);
                            _registro.precio_total = _detalles.Sum(x => x.precio_total);

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        else
                        {
                            //Asignar valores.
                            _registro.costo_unitario = 0;
                            _registro.costo_total = 0;
                            _registro.precio_unitario = 0;
                            _registro.precio_total = 0;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Detalle de Movimiento de Inventario agregado: {id}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return RedirectToAction("Details/" + _registro.PK_codigo);
                    }
                }
                //Actualizar vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"InventariosMovimientosController :: Delete() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return View();
            }
        }

        public ActionResult Procesar(int id)
        {
            try
            {
                //Buscar registro.
                var _factura = db.vw_inventarios_movimientos.Where(x => x.PK_codigo == id).FirstOrDefault();
                var _detalle = db.vw_inventarios_movimientos_detalle.Where(x => x.FK_movimiento == id).ToList();

                //Validar que el modelo no sea null.
                if (_factura != null && _detalle.Count() > 0 && _detalle.Sum(x => x.costo_total) > 0)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Ejecución de procedimiento.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_procesar_inventarios_movimientos", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = id;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Movimiento de inventario procesado: {_factura.PK_codigo}";
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
                var descripcion = $"InventariosMovimientosController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

    }
}