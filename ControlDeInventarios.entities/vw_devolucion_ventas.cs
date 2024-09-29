using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_devolucion_ventas
    {
        [Key]
        public int PK_codigo { get; set; }
        public DateTime fecha { get; set; }
        public int FK_clientes_pedidos { get; set; }
        public decimal total { get; set; }
    }
}
