using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlDeInventarios.entities;

namespace ControlDeInventarios.mvc.Models
{
    public class ResetPassword
    {
        private readonly contexto _contexto;

        public ResetPassword()
        {
            _contexto = new contexto();
        }

        public int PK_codigo { get; set; }
        // int FK_usuario 
        public int FK_usuario { get; set; }
        // varchar(max) hash
        public string hash { get; set; }
        // timestamp vence
        public DateTime vence { get; set; }

        public password_reset getPasswordReset(string _hash)
        {
            return _contexto.password_reset.FirstOrDefault(u => u.hash == _hash);
        }

        public password_reset AddNewRequest (string _correo)
        {
            usuarios user = _contexto.usuarios.FirstOrDefault(u => u.correo == _correo);
            if (user != null)
            {
                password_reset request = new password_reset();
                request.FK_usuario = user.PK_codigo;
                request.hash = createRandomHash();
                request.vence = DateTime.Now.AddHours(1);
                _contexto.password_reset.Add(request);
                _contexto.SaveChanges();

                return request;
            }
            return null;
        }

        public string createRandomHash()
        {
            string abc = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random r = new Random();
            string hash = "";
            for (int i = 0; i < 32; i++)
            {
                hash += abc[r.Next(abc.Length)];
            }
            return hash;
        }
    }
}