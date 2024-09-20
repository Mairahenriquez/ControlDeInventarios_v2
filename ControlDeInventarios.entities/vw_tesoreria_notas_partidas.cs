using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_tesoreria_notas_partidas
    {
        [Key]
        public int PK_codigo { get; set; }
        public int FK_tesoreria_nota { get; set; }
        public int FK_cuenta { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public string concepto { get; set; }
        public int FK_tipo_detalle { get; set; }
        public string cuenta_nombre { get; set; }
    }
}
