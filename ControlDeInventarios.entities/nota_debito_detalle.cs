using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    [Table("nota_debito_detalle")]
    public class nota_debito_detalle
    {
        [Key]
        public int PK_codigo { get; set; }

        public int FK_nota_debito{ get; set; }

        public string concepto { get; set; }
        public decimal total { get; set; }

    }
}
