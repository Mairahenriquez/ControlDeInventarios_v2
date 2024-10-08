using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class tesoreria_cheques
    {
        [Key]
        public int PK_codigo { get; set; }
        public decimal monto { get; set; }
        public DateTime fecha { get; set; }
        public int FK_proveedores_compras { get; set; }
        public int FK_tesoreria_bancos { get; set; }
    }
}
