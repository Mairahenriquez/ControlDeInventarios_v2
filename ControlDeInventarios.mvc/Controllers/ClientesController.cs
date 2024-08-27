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
    public class ClientesController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: Clientes
        public ActionResult Index()
        {
            var _clientes = db.vw_clientes.ToList();
            return View(_clientes);
        }

        public ActionResult Details(int id)
        {
            var _cliente = db.vw_clientes.Where(x => x.PK_codigo == id).FirstOrDefault();

            if (_cliente != null)
            {
                return View(_cliente);
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
        public ActionResult Create(clientes value)
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
                        value.abonos = 0;
                        value.cargos = 0;
                        value.total = 0;
                        value.FK_estado = 1;
                        value.FK_usuario = 1;
                        value.FK_condicion_pago = 1;
                        value.FK_cuenta_contable = 1;
                        value.fecha_nacimiento = DateTime.Now;

                        //Guarda el registro en la base de datos.
                        db.clientes.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cliente agregado: {value.PK_codigo} - {value.nombre}.";
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
                var descripcion = $"ClientesController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _cliente = db.vw_clientes.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_cliente);

        }
        [HttpPost]
        public ActionResult Edit(vw_clientes value)
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
                        var _cliente = db.clientes.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Se igualan valores.
                        _cliente.nombre = value.nombre;
                        _cliente.direccion = value.direccion;
                        _cliente.telefono = value.telefono;
                        _cliente.correo = value.correo;
                        _cliente.nit = value.nit;
                        _cliente.dui = value.dui;
                        _cliente.nrc = value.nrc;
                        _cliente.nombre_comercial = value.nombre_comercial;
                        _cliente.observaciones = value.observaciones;
                        _cliente.FK_municipio = value.FK_municipio;

                        //Se actualiza el registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cliente actualizado: {value.PK_codigo} - {value.nombre}.";
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
                var descripcion = $"ClientesController :: Edit() :: {e.Message}.";
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
                var _cliente = db.clientes.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_cliente != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _cliente.FK_estado = 2;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cliente inactivo: {_cliente.PK_codigo} - {_cliente.nombre}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_cliente);
                    }
                }
                //Actualizar vista.
                return Json(_cliente);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesController :: Desactivar() :: {e.Message}.";
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
                var _cliente = db.clientes.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_cliente != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _cliente.FK_estado = 1;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cliente activo: {_cliente.PK_codigo} - {_cliente.nombre}.";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_cliente);
                    }
                }
                //Actualizar vista.
                return Json(_cliente);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ClientesController :: Activar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }
    }
}