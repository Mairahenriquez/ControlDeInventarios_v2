using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    [Table("devolucion_compra_detalle")]
    public class devolucion_compra_detalle
    {
        [Key]
        public int PK_codigo { get; set; }
        public int FK_devolucion_compra { get; set; }

        public int FK_compra_detalle { get; set; }

    }
}
