using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlDeInventarios.mvc.Models;

namespace ControlDeInventarios.mvc.Middlewares
{
    public class AuthAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["correo"] == null)
            {

                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
            //- get user data
            if (HttpContext.Current.Session["correo"] != null)
            {
                // - check if user is logged in
                Usuario user = new Usuario();
                // - set userData global variable
                HttpContext.Current.Items["userData"] = user.getUsuario(HttpContext.Current.Session["correo"].ToString());
            }

            base.OnActionExecuting(filterContext);
        }


    }
}