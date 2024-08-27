using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class bitacoras
    {
        [Key]
        public int PK_codigo { get; set; }
        public string descripcion { get; set; }
        public int FK_usuario { get; set; }
    }
}
