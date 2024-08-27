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
    public class ProveedoresOrdenesComprasController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: ProveedoresOrdenesCompras
        public ActionResult Index()
        {
            var _ordenes = db.vw_proveedores_ordenes_compras.ToList();
            return View(_ordenes);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _ordenes = db.vw_proveedores_ordenes_compras.Where(x => x.PK_codigo == id).FirstOrDefault();

            //Validar que sea diferente de null.
            if (_ordenes != null)
            {
                ViewBag._detalle = db.vw_proveedores_ordenes_compras_detalle.Where(x => x.FK_orden_compra == id).ToList();
                ViewBag._compras = db.vw_proveedores_compras.Where(x => x.FK_orden_compra == id).ToList();
                return View(_ordenes);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            ViewBag.FormaPago = db.formas_pagos.Where(x => x.PK_codigo == 1).FirstOrDefault();
            ViewBag.CondicionPago = db.condiciones_pagos.Where(x => x.PK_codigo == 1).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult Create(proveedores_ordenes_compras value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Se asignan valores iniciales.
                        value.fecha_hora = DateTime.Now;
                        value.subtotal = 0;
                        value.iva = 0;
                        value.descuento = 0;
                        value.total = 0;
                        value.FK_estado = 1;
                        value.FK_usuario = 1;

                        //Guarda el registro en la base de datos.
                        db.proveedores_ordenes_compras.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Orden de compra agregad: {value.PK_codigo}";
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
                var descripcion = $"ProveedoresOrdenesComprasController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _orden = db.vw_proveedores_ordenes_compras.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_orden);
        }

        [HttpPost]
        public ActionResult Edit(vw_proveedores_ordenes_compras value)
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
                        var _orden = db.proveedores_ordenes_compras.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Se igualan valores.
                        _orden.descripcion = value.descripcion;
                        _orden.fecha = value.fecha;
                        _orden.observaciones = value.observaciones;
                        _orden.FK_bodega = value.FK_bodega;
                        _orden.FK_proveedor = value.FK_proveedor;
                        _orden.FK_forma_pago = value.FK_forma_pago;
                        _orden.FK_condicion_pago = value.FK_condicion_pago;

                        //Se actualiza el registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Orden de compra actualizada: {value.PK_codigo}";
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
                var descripcion = $"ProveedoresOrdenesComprasController :: Edit() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Insert(proveedores_ordenes_compras_detalle value)
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
                        var _orden = db.proveedores_ordenes_compras.Where(x => x.PK_codigo == value.FK_orden_compra).FirstOrDefault();

                        //Asignar valores.
                        value.descripcion = _inventario.descripcion;
                        value.subtotal = value.costo_unitario * value.cantidad;
                        value.iva = value.subtotal * 0.13m;
                        value.total = value.subtotal + value.iva;
                        value.FK_bodega = _orden.FK_bodega;

                        //Agrega el regstro.
                        db.proveedores_ordenes_compras_detalle.Add(value);
                        db.SaveChanges();

                        //Actualiza el registro de orden de compra.
                        var _detalles = db.proveedores_ordenes_compras_detalle.Where(x => x.FK_orden_compra == value.FK_orden_compra).ToList();

                        //Si es mayor a cero.
                        if (_detalles.Count() > 0 && _detalles.Sum(x => x.total) > 0)
                        {
                            //Asignar valores.
                            _orden.subtotal = _detalles.Sum(x => x.subtotal);
                            _orden.iva = _detalles.Sum(x => x.iva);
                            _orden.total = _detalles.Sum(x => x.total);

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        else
                        {
                            //Asignar valores.
                            _orden.subtotal = 0;
                            _orden.iva = 0;
                            _orden.total = 0;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Detalle de orden de compra agregada: {value.PK_codigo}";
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
                var descripcion = $"ProveedoresOrdenesComprasController :: Insert() :: {e.Message}.";
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
                var _detalle = db.proveedores_ordenes_compras_detalle.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_detalle != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Agrega el regstro.
                        db.proveedores_ordenes_compras_detalle.Remove(_detalle);
                        db.SaveChanges();

                        //Actualiza el registro de orden de compra.
                        var _detalles = db.proveedores_ordenes_compras_detalle.Where(x => x.FK_orden_compra == _detalle.FK_orden_compra).ToList();
                        var _orden = db.proveedores_ordenes_compras.Where(x => x.PK_codigo == _detalle.FK_orden_compra).FirstOrDefault();

                        //Si es mayor a cero.
                        if (_detalles.Count() > 0 && _detalles.Sum(x => x.total) > 0 && _orden != null)
                        {
                            //Asignar valores.
                            _orden.subtotal = _detalles.Sum(x => x.subtotal);
                            _orden.iva = _detalles.Sum(x => x.iva);
                            _orden.total = _detalles.Sum(x => x.total);

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        else
                        {
                            //Asignar valores.
                            _orden.subtotal = 0;
                            _orden.iva = 0;
                            _orden.total = 0;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Detalle de orden de compra agregada: {id}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return RedirectToAction("Details/" + _orden.PK_codigo);
                    }
                }
                //Actualizar vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ProveedoresOrdenesComprasController :: Delete() :: {e.Message}.";
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
                var _orden = db.proveedores_ordenes_compras.Where(x => x.PK_codigo == id).FirstOrDefault();
                var _detalle = db.proveedores_ordenes_compras_detalle.Where(x => x.FK_orden_compra == id).ToList();

                //Validar que el modelo no sea null.
                if (_orden != null && _detalle.Sum(x => x.total) > 0)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Ejecución de procedimiento.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_agregar_factura_compras", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = id;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Orden de compra procesada: {_orden.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_orden);
                    }
                }
                //Actualizar vista.
                return Json(_orden);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ProveedoresOrdenesComprasController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

        public ActionResult ValidarDatos(int id)
        {
            try
            {
                //Buscar registro.
                var _registro = db.vw_inventarios.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_registro != null)
                {
                    _registro.precio_unitario = _registro.precio_unitario == 0 ? (_registro.costo_unitario + (_registro.costo_unitario * (_registro.porcentaje_ganacia / 100))) : _registro.precio_unitario;
                    //Retorna hacia la pantalla de Detalle.
                    return Json(_registro);
                }
                //Actualizar vista.
                return Json(null);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesPedidosController :: ValidarDatos() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(null);
            }
        }
    }
}