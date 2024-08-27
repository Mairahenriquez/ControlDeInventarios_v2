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
    public class ContabilidadPartidasController : Controller
    {
        contexto db = new contexto();
        BitacorasController bt = new BitacorasController();
        // GET: ContabilidadPartidas
        public ActionResult Index()
        {
            var _partidas = db.vw_contabilidad_partidas.ToList();
            return View(_partidas);
        }

        public ActionResult Details(int id)
        {
            //Buscar registro.
            var _partida = db.vw_contabilidad_partidas.Where(x => x.PK_codigo == id).FirstOrDefault();

            //Validar que sea diferente de null.
            if (_partida != null)
            {
                ViewBag._detalle = db.vw_contabilidad_partidas_detalle.Where(x => x.FK_partida == id).ToList();
                return View(_partida);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Procesar(int id)
        {
            try
            {
                //Buscar registro.
                var _partida = db.contabilidad_partidas.Where(x => x.PK_codigo == id).FirstOrDefault();

                //Validar que el modelo no sea null.
                if (_partida != null)
                {
                    // Validar que el DataAnnotation sea valido.
                    if (ModelState.IsValid)
                    {
                        //Asignación de valor.
                        _partida.FK_estado = 2;
                        db.SaveChanges();

                        //Guarda en bitacora.
                        var descripcion = $"Partida contable procesada: {_partida.PK_codigo}";
                        var FK_usuario = 1;
                        bt.Create(descripcion, FK_usuario);

                        //Retorna hacia la pantalla de Detalle.
                        return Json(_partida);
                    }
                }
                //Actualizar vista.
                return Json(_partida);
            }
            catch (Exception e)
            {
                //Guarda en bitacora.
                var descripcion = $"ContabilidadPartidasController :: Procesar() :: {e.Message}.";
                bt.Create(descripcion, 1);

                //Actualizar vista.
                return Json(id);
            }
        }

    }
}