using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_tesoreria_notas
    {
        [Key]
        public int PK_codigo { get; set; }
        public string PK_hash { get; set; }
        public string referencia { get; set; }
        public decimal saldo_inicial { get; set; }
        public decimal monto { get; set; }
        public decimal saldo_final { get; set; }
        public DateTime fecha_hora { get; set; }
        public DateTime fecha { get; set; }
        public string descripcion { get; set; }
        public Boolean abono { get; set; }
        public Boolean conciliado { get; set; }
        public DateTime? fecha_conciliado { get; set; }
        public string observaciones { get; set; }
        public decimal monto_facturas { get; set; }
        public decimal monto_partidas { get; set; }
        public int FK_usuario { get; set; }
        public int FK_cuenta_corriente { get; set; }
        public int FK_estado { get; set; }
        public string cuenta_corriente { get; set; }
        public string estado { get; set; }
        public string estado_color { get; set; }
    }
}
