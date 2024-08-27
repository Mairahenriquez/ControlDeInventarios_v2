using ControlDeInventarios.entities;
using ControlDeInventarios.mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDeInventarios.mvc.Controllers
{
    public class BitacorasController : Controller
    {
        contexto db = new contexto();
        bitacoras _bitacora = new bitacoras();
        // GET: Bitacoras
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string descripcion, int FK_usuario)
        {
            try
            {
                //Asignar valores.
                _bitacora.descripcion = (descripcion != null && descripcion != "") ? descripcion : "Descripcion";
                _bitacora.FK_usuario = FK_usuario;

                //Se valida que el modelo no sea nulo.
                if (_bitacora != null)
                {
                    //Se valida el DataAnnotation que sea valido.
                    if (ModelState.IsValid)
                    {
                        //Guarda el registro en la base de datos.
                        db.bitacoras.Add(_bitacora);
                        db.SaveChanges();

                        //Redirecciona a la vista.
                        return View();
                    }
                }
                //Actualiza la vista.
                return View();
            }
            catch (Exception e)
            {
                //Actualiza la vista.
                return View();
            }
        }
    }
}