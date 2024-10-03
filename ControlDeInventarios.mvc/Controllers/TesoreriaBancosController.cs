using ControlDeInventarios.mvc.Middlewares;
using ControlDeInventarios.mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlDeInventarios.entities;

namespace ControlDeInventarios.mvc.Controllers
{
    [AuthAttribute]
    public class TesoreriaBancosController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: TesoreriaBancos
        public ActionResult Index()
        {
            var _registro = db.vw_tesoreria_bancos.ToList();
            return View(_registro);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _registro = db.vw_tesoreria_bancos.Where(x => x.PK_codigo == id).FirstOrDefault();

            if (_registro != null)
            {
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
        public ActionResult Create(tesoreria_bancos value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignación de valores.
                        value.saldo_actual = 0;
                        value.activo = 1;

                        //Guardar registro.
                        db.tesoreria_bancos.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cuenta Bancaria agregada: {value.PK_codigo} - {value.nombre}.";
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
                var descripcion = $"TesoreriaBancosController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _registro = db.vw_tesoreria_bancos.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_registro);

        }

        [HttpPost]
        public ActionResult Edit(vw_tesoreria_bancos value)
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
                        var _registro = db.tesoreria_bancos.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Igualar valores.
                        _registro.nombre = value.nombre;
                        _registro.numero = value.numero;
                        _registro.saldo_inicial = value.saldo_inicial;
                        _registro.fecha_inicial = value.fecha_inicial;
                        _registro.FK_cuenta_contable = value.FK_cuenta_contable;

                        //Guardar registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cuenta Bancaria actualizada: {value.PK_codigo} - {value.nombre}.";
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
                var descripcion = $"TesoreriaBancosController :: Edit() :: {e.Message}.";
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
                var _registro = db.tesoreria_bancos.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_registro != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _registro.activo = false;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cuenta Bancaria inactiva: {_registro.PK_codigo} - {_registro.nombre}.";
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
                var descripcion = $"TesoreriaBancosController :: Desactivar() :: {e.Message}.";
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
                var _registro = db.tesoreria_bancos.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_registro != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignar valores.
                        _registro.activo = true;

                        //Actualizar regstro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cuenta Bancaria activa: {_registro.PK_codigo} - {_registro.nombre}.";
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
                var descripcion = $"TesoreriaBancosController :: Activar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

    }
}