using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class tesoreria_bancos
    {
        [Key]
        public int PK_codigo { get; set; }
        public string nombre { get; set; }
        public string numero { get; set; }
        public Boolean activo { get; set; }
        public decimal saldo_inicial { get; set; }
        public decimal saldo_actual { get; set; }
        public DateTime fecha_inicial { get; set; }
        public int FK_cuenta_contable { get; set; }
    }
}
