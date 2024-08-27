using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_paises_municipios
    {
        [Key]
        public int PK_codigo { get; set; }
        public string nombre { get; set; }
        public int FK_departamento { get; set; }
        public string departamento { get; set; }
    }
}
