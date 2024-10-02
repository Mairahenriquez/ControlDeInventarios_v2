using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_clientes_abonos
    {
        [Key]
        public int PK_codigo { get; set; }
        public Guid PK_hash { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fecha_hora { get; set; }
        public decimal monto { get; set; }
        public string referencia { get; set; }
        public string observaciones { get; set; }
        public Boolean conciliado { get; set; }
        public DateTime? fecha_conciliado { get; set; }
        public Boolean ingreso_manual { get; set; }
        public int? FK_factura { get; set; }
        public int FK_forma_pago { get; set; }
        public int FK_usuario { get; set; }
        public int FK_estado { get; set; }
        public int? FK_cuenta_corriente { get; set; }
        public string forma_pago { get; set; }
        public string estado { get; set; }
        public string estado_color { get; set; }
        public string cuenta_corriente { get; set; }
        public string cliente { get; set; }
    }
}
