using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlDeInventarios.mvc.Models;
using ControlDeInventarios.mvc.Controllers;
using ControlDeInventarios.entities;
using ControlDeInventarios.mvc.Middlewares;

namespace ControlDeInventarios.mvc.Controllers.Admin
{
    [AuthAttribute]
    public class TesoreriaChequesController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: TesoreriaBancos
        public ActionResult Index()
        {
            var _registro = db.vw_tesoreria_cheques.ToList();
            return View(_registro);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _registro = db.vw_tesoreria_cheques.Where(x => x.PK_codigo == id).FirstOrDefault();
         
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
            var _bancos = db.vw_tesoreria_bancos.ToList();
            var _cheque = db.formas_pagos.Where(x => x.nombre == "Cheque").First();
            var _orden_compra = db.vw_proveedores_compras.Where(x => x.FK_forma_pago == _cheque.PK_codigo);
            ViewBag._bancos = _bancos;
            ViewBag._orden_compra = _orden_compra;
            return View();
        }

        [HttpPost]
        public ActionResult Create(tesoreria_cheques value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                       
                        //Guardar registro.
                        db.tesoreria_cheques.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cheque emitido correctamente: por un valor de {value.monto}.";
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
            var _registro = db.tesoreria_cheques.Where(x => x.PK_codigo == id).FirstOrDefault();
            var _bancos = db.vw_tesoreria_bancos.ToList();
            var _cheque = db.formas_pagos.Where(x => x.nombre == "Cheque").First();
            var _orden_compra = db.vw_proveedores_compras.Where(x => x.FK_forma_pago == _cheque.PK_codigo);
            ViewBag._bancos = _bancos;
            ViewBag._orden_compra = _orden_compra;
            return View(_registro);

        }

        [HttpPost]
        public ActionResult Edit(tesoreria_cheques value)
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
                        var _registro = db.tesoreria_cheques.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Igualar valores.
                        _registro.FK_tesoreria_bancos = value.FK_tesoreria_bancos;
                        _registro.FK_proveedores_compras = value.FK_proveedores_compras;
                        _registro.monto = value.monto;
                        _registro.fecha = value.fecha;

                        //Guardar registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cheque actualizado: por un valor de {value.monto}.";
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

      
        
    }
}