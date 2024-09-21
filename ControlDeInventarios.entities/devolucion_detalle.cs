using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    [Table("devolucion_compra_detalles")]
    public class devolucion_compra_detalle
    {
        public int PK_codigo { get; set; }

        public int 

        public string observaciones { get; set; }

        public int FK_compra { get; set; }

    }
}
