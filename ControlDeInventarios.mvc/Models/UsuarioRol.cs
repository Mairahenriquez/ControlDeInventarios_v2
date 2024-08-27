using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlDeInventarios.entities;

namespace ControlDeInventarios.mvc.Models
{
    public class UsuarioRol
    {
        private readonly contexto _contexto;

        public UsuarioRol ()
        {
            _contexto = new contexto();
        }

        public int PK_codigo { get; set; }
        public string nombre { get; set; }

        public usuarios_roles getRol(int _codigo)
        {
            return  _contexto.usuarios_roles.FirstOrDefault(u => u.PK_codigo == _codigo);
        }

    }
}