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
    public class UsuarioController : Controller
    {
        public contexto db = new contexto();
        // GET: Usuario

        public ActionResult Index()
        {
            return View("~/Views/Admin/Usuario/Index.cshtml");
        }
        public ActionResult Crear ()
        {
            // get all roles
            List<usuarios_roles> roles = db.usuarios_roles.ToList();
            ViewData["roles"] = roles;
            return View("~/Views/Admin/Usuario/Crear.cshtml");
        }

        [HttpPost]
        public ActionResult PostRegister(Usuario rUser)
        {
            // email exists
            if (db.usuarios.Any(u => u.correo == rUser.correo))
            {
                TempData["error_message"] = "El email ya existe";
                return RedirectToAction("Crear", "Usuario");
            }

            // password match
            if (rUser.clave == rUser.confirmClave)
            {
                rUser.clave = AuthController.ConvertirSha256(rUser.clave);
            }
            else
            {
                TempData["error_message"] = "Las contraseñas no coinciden";
                return RedirectToAction("Crear", "Usuario");
            }

            // register user
            try
            {
                usuarios newUser = new usuarios();
                newUser.usuario = rUser.usuario;
                newUser.nombre = rUser.nombre;
                newUser.clave = rUser.clave;
                newUser.telefono = rUser.telefono;
                newUser.correo = rUser.correo;
                newUser.FK_estado = 1;
                newUser.FK_rol = rUser.FK_rol;
                
                db.usuarios.Add(newUser);
                db.SaveChanges();
                TempData["success_message"] = "Usuario registrado correctamente";
                return RedirectToAction("Crear", "Usuario");
            }
            catch (Exception e)
            {
                TempData["success_message"] = "Error al registrar usuario";
                return RedirectToAction("Crear", "Usuario");
            }

            TempData["success_message"] = "Error al registrar usuario";
            return RedirectToAction("Crear", "Usuario");

        }


    }
}