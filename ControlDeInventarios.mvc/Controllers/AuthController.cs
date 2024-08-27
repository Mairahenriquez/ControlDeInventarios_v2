using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlDeInventarios.mvc.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using ControlDeInventarios.entities;
using ControlDeInventarios.mvc.Utils;

namespace ControlDeInventarios.mvc.Controllers
{

    public class AuthController : Controller
    {
        contexto db = new contexto();
        // GET: Auth
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ResetPassword ()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HashResetPassword(string id)
        {
            ResetPassword aux = new ResetPassword();
            password_reset reset = aux.getPasswordReset(id);
            if (reset != null)
            {
                if (reset.vence > DateTime.Now)
                {
                    TempData["hash"] = id;
                    return View("~/Views/Auth/ResetPassword.cshtml");
                }
                TempData["error_message"] = "El enlace ha expirado.";
                return RedirectToAction("ResetPassword", "Auth");
            }
            TempData["error_message"] = "El enlace no es válido." + id;
            return View("~/Views/Auth/ResetPassword.cshtml");
        }
        [HttpPost]
        public ActionResult UpdatePassword (string id, string clave, string confirmClave)
        {
            if (clave == null || confirmClave == null)
            {
                TempData["error_message"] = "No se aceptan contraseñas vacias.";
                return RedirectToAction("ResetPassword", "Auth");
            }
            if (clave != confirmClave)
            {
                TempData["error_message"] = "Las contraseñas no coinciden.";
                return RedirectToAction("ResetPassword", "Auth");
            }
            ResetPassword aux = new ResetPassword();
            password_reset reset = aux.getPasswordReset(id);
            if (reset != null)
            {
                if (reset.vence > DateTime.Now)
                {
                    usuarios user = db.usuarios.FirstOrDefault(u => u.PK_codigo == reset.FK_usuario);
                    user.clave = ConvertirSha256(clave);
                    // expire hash
                    reset.vence = DateTime.Now;
                    db.SaveChanges();
                    TempData["success_message"] = "Contraseña restablecida correctamente.";
                    return RedirectToAction("Login", "Auth");
                }
                TempData["error_message"] = "El enlace ha expirado.";
                return RedirectToAction("ResetPassword", "Auth");
            }
            TempData["error_message"] = "El enlace no es válido." + id;
            return RedirectToAction("ResetPassword", "Auth");
        }


        [HttpPost]
        public ActionResult SetResetPassword(string correo)
        {
            if (correo == null)
            {
                TempData["error_message"] = "No se aceptan correos vacios" + correo;
                return RedirectToAction("ResetPassword", "Auth");
            }
            ResetPassword aux = new ResetPassword();
            password_reset reset = aux.AddNewRequest(correo);
            if (reset != null)
            {
                string body = BodyResetPasswordHtml(reset.hash);
                EmailSender email = new EmailSender();
                email.SendEmail(correo, "Restablecer contraseña", body);
                TempData["success_message"] = "Se ha enviado un correo con las instrucciones para restablecer la contraseña.";
                return RedirectToAction("ResetPassword", "Auth");
            }
            TempData["error_message"] = "No se ha encontrado el correo en la base de datos.";
            return RedirectToAction("ResetPassword", "Auth");
        }

        
        [HttpPost]
        public ActionResult PostLogin(usuarios rUser)
        {
            bool isLogin;
            string message;

            if (rUser.correo == null || rUser.clave == null)
            {
                TempData["error_login"] = "No puede dejar los campos vacios.";
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                // email exists
                usuarios getUsuario = db.usuarios.FirstOrDefault(u => u.correo == rUser.correo);
                if (getUsuario != null)
                {
                    // password match
                    if (getUsuario.clave == ConvertirSha256(rUser.clave))
                    {
                        Session["correo"] = rUser.correo;
                        return RedirectToAction("Index", "Home");
                        // 
                    }
                }
            } catch (Exception e)
            {
                TempData["error_login"] = "Error al iniciar sesión." + e.Data.ToString() ;
                return RedirectToAction("Login", "Auth");
            }
            TempData["error_login"] = "Datos incorrectos.";
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session["correo"] = null;
            Session["UserData"] = null;
            return RedirectToAction("Login", "Auth");
        }

        public static string ConvertirSha256(string texto)
        {

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string BodyResetPasswordHtml (string hash)
        {
            string body = "<h1>Restablecer contraseña</h1>";
            body += "<p>Para restablecer tu contraseña, haz clic en el siguiente enlace: <a href='https://localhost:44343/Auth/HashResetPassword/" + hash + "'>Restablecer contraseña</a></p>";
            return body;
        }
    }
}