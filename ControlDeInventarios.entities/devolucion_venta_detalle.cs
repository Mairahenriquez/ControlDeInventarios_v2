using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    [Table("devolucion_venta_detalle")]
    public class devolucion_venta_detalle
    {
        [Key]
        public int PK_codigo { get; set; }
        public int FK_pedido_detalle { get; set; }
        public int FK_devolucion_ventas { get; set; }

    }
}
