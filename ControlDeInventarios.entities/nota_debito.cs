using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    [Table("nota_debito")]
    public class nota_debito
    {
        [Key]
        public int PK_codigo { get; set; }

        public int FK_facturacion { get; set; }

        public DateTime fecha_hora { get; set; }

        public string numero { get; set; }

        public decimal total { get; set; }

        public string observaciones { get; set; }


    }
}
