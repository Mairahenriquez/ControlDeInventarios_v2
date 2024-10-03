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
    public class ClientesPedidosController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: ClientesPedidos
        public ActionResult Index()
        {
            var _pedidos = db.vw_clientes_pedidos.ToList();
            return View(_pedidos);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _pedido = db.vw_clientes_pedidos.Where(x => x.PK_codigo == id).FirstOrDefault();

            //Validar que sea diferente de null.
            if (_pedido != null)
            {
                ViewBag._detalle = db.vw_clientes_pedidos_detalle.Where(x => x.FK_pedido == id).ToList();
                ViewBag._facturas = db.vw_facturacion.Where(x => x.FK_pedido == id).ToList();
                return View(_pedido);
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
        public ActionResult Create(clientes_pedidos value)
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
                        value.subtotal = 0;
                        value.iva = 0;
                        value.total = 0;
                        value.FK_estado = 1;
                        value.FK_usuario = 1;
                        value.FK_forma_pago = 1;

                        //Guarda el registro en la base de datos.
                        db.clientes_pedidos.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Pedido agregado: {value.PK_codigo}";
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
                var descripcion = $"ClientesPedidosController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _pedido = db.vw_clientes_pedidos.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_pedido);

        }

        [HttpPost]
        public ActionResult Edit(vw_clientes_pedidos value)
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
                        var _orden = db.clientes_pedidos.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Se igualan valores.
                        _orden.fecha = value.fecha;
                        _orden.referencia = value.referencia;
                        _orden.observaciones = value.observaciones;
                        _orden.FK_cliente = value.FK_cliente;
                        _orden.FK_condicion_pago = value.FK_condicion_pago;

                        //Se actualiza el registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Pedido actualizado: {value.PK_codigo}";
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
                var descripcion = $"ClientesPedidosController :: Edit() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Insert(clientes_pedidos_detalle value)
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
                        var _pedido = db.clientes_pedidos.Where(x => x.PK_codigo == value.FK_pedido).FirstOrDefault();

                        //Asignar valores.
                        value.descripcion = _inventario.descripcion;
                        value.subtotal = value.precio_unitario * value.cantidad;
                        value.iva = value.subtotal * 0.13m;
                        value.total = value.subtotal + value.iva;
                        value.FK_bodega = 1;

                        //Agrega el regstro.
                        db.clientes_pedidos_detalle.Add(value);
                        db.SaveChanges();

                        //Actualiza el registro de orden de compra.
                        var _detalles = db.clientes_pedidos_detalle.Where(x => x.FK_pedido == value.FK_pedido).ToList();

                        //Si es mayor a cero.
                        if (_detalles.Count() > 0 && _detalles.Sum(x => x.total) > 0)
                        {
                            //Asignar valores.
                            _pedido.subtotal = _detalles.Sum(x => x.subtotal);
                            _pedido.iva = _detalles.Sum(x => x.iva);
                            _pedido.total = _detalles.Sum(x => x.total);

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        else
                        {
                            //Asignar valores.
                            _pedido.subtotal = 0;
                            _pedido.iva = 0;
                            _pedido.total = 0;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Detalle de pedido agregado: {value.PK_codigo}";
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
                var descripcion = $"ClientesPedidosController :: Insert() :: {e.Message}.";
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
                var _detalle = db.clientes_pedidos_detalle.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_detalle != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Agrega el regstro.
                        db.clientes_pedidos_detalle.Remove(_detalle);
                        db.SaveChanges();

                        //Actualiza el registro de orden de compra.
                        var _detalles = db.clientes_pedidos_detalle.Where(x => x.FK_pedido == _detalle.FK_pedido).ToList();
                        var _pedido = db.clientes_pedidos.Where(x => x.PK_codigo == _detalle.FK_pedido).FirstOrDefault();

                        //Si es mayor a cero.
                        if (_detalles.Count() > 0 && _detalles.Sum(x => x.total) > 0 && _pedido != null)
                        {
                            //Asignar valores.
                            _pedido.subtotal = _detalles.Sum(x => x.subtotal);
                            _pedido.iva = _detalles.Sum(x => x.iva);
                            _pedido.total = _detalles.Sum(x => x.total);

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        else
                        {
                            //Asignar valores.
                            _pedido.subtotal = 0;
                            _pedido.iva = 0;
                            _pedido.total = 0;

                            //Actualizar registro.
                            db.SaveChanges();
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Detalle de pedido agregado: {id}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return RedirectToAction("Details/" + _pedido.PK_codigo);
                    }
                }
                //Actualizar vista.
                return View();
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesPedidosController :: Delete() :: {e.Message}.";
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
                var _orden = db.clientes_pedidos.Where(x => x.PK_codigo == id).FirstOrDefault();
                var _detalle = db.vw_clientes_pedidos_detalle.Where(x => x.FK_pedido == id).ToList();

                //Validar que el modelo no sea null.
                if (_orden != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (_detalle.Sum(x => x.total) > 0)
                    {
                        //Ejecución de procedimiento.
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_agregar_factura_ventas", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@PK_codigo", SqlDbType.Int).Value = id;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //Guarda en bitacora.
                        var descripcion = $"Pedido procesado: {_orden.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Actualiza la vista.
                        return new JsonResult { Data = new { result = 1, message = "" } };
                    }
                    //Actualiza la vista.
                    return new JsonResult { Data = new { result = 0, message = "No se puede procesar el pedido, verificar que el total sea mayor a cero." } };
                }
                //Actualiza la vista.
                return new JsonResult { Data = new { result = 0, message = "No se puede procesar el pedido, verificar que existan productos seleccionados." } };
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesPedidosController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la página.
                return new JsonResult { Data = new { result = 0, message = "No se puede procesar el abono." } };
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
                    _registro.precio_unitario = _registro.precio_unitario == 0 ? (_registro.costo_unitario + (_registro.costo_unitario * (_registro.porcentaje_ganacia / 100))) : _registro.precio_unitario / 1.13m;
                    
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