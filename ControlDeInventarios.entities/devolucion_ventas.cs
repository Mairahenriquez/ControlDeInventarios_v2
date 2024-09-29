using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    [Table("devolucion_ventas")]
    public class devolucion_ventas
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PK_codigo { get; set; }
        public int FK_clientes_pedidos{ get; set; }

        public DateTime fecha { get; set; }

        public string observaciones { get; set; }

    }
}
