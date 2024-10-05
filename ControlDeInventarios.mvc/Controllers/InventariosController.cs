using ControlDeInventarios.entities;
using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class InventariosController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: Inventarios
        public ActionResult Index()
        {
            var _inventarios = db.vw_inventarios.OrderByDescending(x => x.PK_codigo).ToList();
            return View(_inventarios);
        }

        public ActionResult Details(int id)
        {
            var _inventario = db.vw_inventarios.Where(x => x.PK_codigo == id).FirstOrDefault();
            ViewBag._existencias = db.vw_inventarios_existencias_por_bodegas.Where(x => x.FK_inventario == id).ToList();

            if (_inventario != null)
            {
                return View(_inventario);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            ViewBag.CuentaInventarios = db.vw_contabilidad_cuentas_contables.Where(x => x.numero == "1105").FirstOrDefault();
            ViewBag.CuentaCostoVenta = db.vw_contabilidad_cuentas_contables.Where(x => x.numero == "4101").FirstOrDefault();
            ViewBag.CuentaIngresoVenta = db.vw_contabilidad_cuentas_contables.Where(x => x.numero == "5101").FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult Create(inventarios value)
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
                        value.imagen = "/img/no_image.jpg";
                        value.precio_unitario = 0;
                        value.costo_unitario = 0;
                        value.precio_cif = 0;
                        value.precio_fob = 0;
                        value.precio_total = 0;
                        value.costo_total = 0;
                        value.descuento = 0;
                        value.existencia_fisica = 0;
                        value.FK_estado = 1;
                        value.FK_anio = 1;
                        value.FK_estado_fisico = 1;
                        value.FK_cuenta_contable_devoluciones = value.FK_cuenta_contable_inventarios;

                        //Guarda el registro en la base de datos.
                        db.inventarios.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Inventario agregado: {value.PK_codigo} - {value.descripcion}.";
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
                var descripcion = $"InventariosController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _inventario = db.vw_inventarios.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_inventario);

        }

        [HttpPost]
        public ActionResult Edit(vw_inventarios value)
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
                        var _inventario = db.inventarios.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Se igualan valores.
                        _inventario.identificador = value.identificador;
                        _inventario.descripcion = value.descripcion;
                        _inventario.observaciones = value.observaciones;
                        _inventario.fecha_vencimiento = value.fecha_vencimiento;
                        _inventario.comprable = value.comprable;
                        _inventario.vendible = value.vendible;
                        _inventario.FK_cuenta_contable_inventarios = value.FK_cuenta_contable_inventarios;
                        _inventario.FK_cuenta_contable_costo_venta = value.FK_cuenta_contable_costo_venta;
                        _inventario.FK_cuenta_contable_ingreso_venta = value.FK_cuenta_contable_ingreso_venta;
                        _inventario.FK_cuenta_contable_devoluciones = value.FK_cuenta_contable_inventarios;
                        _inventario.porcentaje_ganacia = value.porcentaje_ganacia;

                        //Se actualiza el registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Inventario actualizado: {value.PK_codigo} - {value.descripcion}.";
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
                var descripcion = $"InventariosController :: Edit() :: {e.Message}.";
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
                var _inventario = db.inventarios.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_inventario != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _inventario.FK_estado = 2;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Inventario inactivo: {_inventario.PK_codigo} - {_inventario.descripcion}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_inventario);
                    }
                }
                //Actualizar vista.
                return Json(_inventario);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"InventariosController :: Desactivar() :: {e.Message}.";
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
                var _inventario = db.inventarios.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_inventario != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _inventario.FK_estado = 1;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Inventario activo: {_inventario.PK_codigo} - {_inventario.descripcion}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_inventario);
                    }
                }
                //Actualizar vista.
                return Json(_inventario);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"InventariosController :: Activar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

        public ActionResult Libro_diario(DateTime? fecha_inicio = null)
        {
            var fecha = DateTime.Now.Date;
            var _partidas = db.vw_contabilidad_partidas_detalle.ToList();
            return View(_partidas);
            //if (fecha_inicio != null)
            //{
            //    var _partidas = db.vw_contabilidad_partidas_detalle.Where(x => x.fecha == fecha_inicio).ToList();
            //    return View(_partidas);
            //}
            //else
            //{
            //    var _partidas = db.vw_contabilidad_partidas_detalle.Where(x => x.fecha == fecha).ToList();
            //    return View(_partidas);
            //}
        }

        public ActionResult Libro_mayor(DateTime? fecha_inicio = null, DateTime? fecha_final = null)
        {
            if (fecha_inicio != null && fecha_final != null)
            {
                var _partidas = db.vw_contabilidad_partidas_detalle.Where(x => x.fecha > fecha_inicio.Value.Date && x.fecha < fecha_final.Value.Date).ToList();
                return Json(_partidas);
            }
            else
            {
                var _partidas = new List<vw_contabilidad_partidas_detalle>();
                return Json(_partidas);
            }
        }
    }
}