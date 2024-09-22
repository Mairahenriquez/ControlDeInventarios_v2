using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    [Table("devolucion_compras")]
    public class devolucion_compra
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PK_codigo { get; set; }
        public int FK_proveedores_compras { get; set; }

        public DateTime fecha { get; set; }

        public string observaciones { get; set; }

    }
}
