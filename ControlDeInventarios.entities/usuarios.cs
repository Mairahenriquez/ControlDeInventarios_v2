using System;
using System.ComponentModel.DataAnnotations;

namespace ControlDeInventarios.entities
{
    // columns [PK_codigo] ,[usuario] ,[nombre] ,[clave] ,[telefono] ,[correo] ,[FK_estado] ,[FK_usuario] ,[FK_rol]

    public class usuarios
    {

        [Key]
        public int PK_codigo { get; set; }

        public string usuario { get; set; }
        public string nombre { get; set; }
        public string clave { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public int FK_estado { get; set; }
        public int FK_rol { get; set; }

        public string confirmClave;
    }
}
