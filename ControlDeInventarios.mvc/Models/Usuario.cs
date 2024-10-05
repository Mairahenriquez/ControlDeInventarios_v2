using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlDeInventarios.entities;

namespace ControlDeInventarios.mvc.Models
{
    public class Usuario
    {
        // columns [PK_codigo] ,[usuario] ,[nombre] ,[clave] ,[telefono] ,[correo] ,[FK_estado] ,[FK_usuario] ,[FK_rol]
        private readonly contexto _context;

        public Usuario()
        {
            _context = new contexto();
        }

        public Usuario (string _correo)
        {
            _context = new contexto();
            usuarios user = getUsuario(_correo);
            PK_codigo = user.PK_codigo;
            usuario = user.usuario;
            nombre = user.nombre;
            clave = user.clave;
            telefono = user.telefono;
            correo = user.correo;
            FK_estado = user.FK_estado;
            FK_rol = user.FK_rol;
        }

        public int PK_codigo { get; set; }
        public int codigo { get; set; }
        public string usuario { get; set; }
        public string nombre { get; set; }
        public string clave { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }

        public int FK_estado { get; set; }
        public int FK_rol { get; set; }

        // Optional variables
        public string confirmClave { get; set; }
        public bool rememberMe { get; set; }

        public bool isAdmin ()
        {
            usuarios_roles rol = (new UsuarioRol()).getRol(FK_rol);
            return rol.nombre == "admin";
        }

        public bool IsGerente()
        {
            usuarios_roles rol = (new UsuarioRol()).getRol(FK_rol);
            return rol.nombre == "gerente";
        }

        public bool IsContador()
        {
            usuarios_roles rol = (new UsuarioRol()).getRol(FK_rol);
            return rol.nombre == "contador";
        }

        public bool IsVendedor()
        {
            usuarios_roles rol = (new UsuarioRol()).getRol(FK_rol);
            return rol.nombre == "vendedor";
        }

        public bool IsAtencionCliente()
        {
            usuarios_roles rol = (new UsuarioRol()).getRol(FK_rol);
            return rol.nombre == "atención al cliente";
        }

        public usuarios getUsuario(string _correo)
        {
            return  _context.usuarios.FirstOrDefault(u => u.correo == _correo);
        }



        // Override ToString
        public override string ToString()
        {
            return "Usuario: " + usuario + " Nombre: " + nombre + " Correo: " + correo + " Telefono: " + telefono + " Rol: " + FK_rol + " Estado: " + FK_estado + " Clanve: " + clave + " ConfirmClave: " + confirmClave + " RememberMe: " + rememberMe;
        }
    }
}