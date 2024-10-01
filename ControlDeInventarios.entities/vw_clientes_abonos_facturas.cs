using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_clientes_abonos_facturas
    {
        [Key]
        public int PK_codigo { get; set;} 
        public decimal monto { get; set;} 
        public string referencia { get; set;} 
        public string observaciones { get; set;} 
        public int FK_abono { get; set;} 
        public int FK_factura { get; set;} 
        public decimal saldo { get; set;} 
        public string cliente { get; set;} 
    }
}
