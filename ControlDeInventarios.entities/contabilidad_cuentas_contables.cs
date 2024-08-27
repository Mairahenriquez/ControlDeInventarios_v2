using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class contabilidad_cuentas_contables
    {
        [Key]
        public int PK_codigo { get; set; }
        public string numero { get; set; }
        public string nombre { get; set; }
        public Boolean cuenta_mayor { get; set; }
        public int? FK_cuenta_contable { get; set; }
        public int FK_clasificasion { get; set; }
    }
}
