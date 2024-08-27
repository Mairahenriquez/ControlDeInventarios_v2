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
    public class ContabilidadCuentasContablesController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: ContabilidadCuentasContables
        public ActionResult Index()
        {
            var _cuentas = db.vw_contabilidad_cuentas_contables.OrderBy(x => x.FK_clasificasion).ToList();
            return View(_cuentas);
        }

        public ActionResult Details(int id)
        {
            var _cuenta = db.vw_contabilidad_cuentas_contables.Where(x => x.PK_codigo == id).FirstOrDefault();

            if (_cuenta != null)
            {
                return View(_cuenta);
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
        public ActionResult Create(contabilidad_cuentas_contables value)
        {
            try
            {
                //Se valida que el modelo no sea nulo.
                if (value != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Guarda el registro en la base de datos.
                        db.contabilidad_cuentas_contables.Add(value);
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cuenta contable agregada: {value.numero} - {value.nombre}.";
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
                var descripcion = $"ContabilidadCuentasContablesController :: Create() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var _cuenta = db.vw_contabilidad_cuentas_contables.Where(x => x.PK_codigo == id).FirstOrDefault();
            return View(_cuenta);

        }
        [HttpPost]
        public ActionResult Edit(vw_contabilidad_cuentas_contables value)
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
                        var _cuenta = db.contabilidad_cuentas_contables.Where(x => x.PK_codigo == value.PK_codigo).FirstOrDefault();

                        //Se igualan valores.
                        _cuenta.numero = value.numero;
                        _cuenta.nombre = value.nombre;
                        _cuenta.cuenta_mayor = value.cuenta_mayor;
                        _cuenta.FK_cuenta_contable = value.FK_cuenta_contable;
                        _cuenta.FK_clasificasion = value.FK_clasificasion;

                        //Se actualiza el registro.
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Cuenta contable actualizada: {value.numero} - {value.nombre}.";
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
                var descripcion = $"ContabilidadCuentasContablesController :: Edit() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualiza la vista.
                return View();
            }
        }

    }
}