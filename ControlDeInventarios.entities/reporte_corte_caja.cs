using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class reporte_corte_caja
    {
        [Key]
        public int PK_codigo { get; set; }
        public string tipo { get; set; }
        public string forma_pago { get; set; }
        public decimal total { get; set; }
    }
}
