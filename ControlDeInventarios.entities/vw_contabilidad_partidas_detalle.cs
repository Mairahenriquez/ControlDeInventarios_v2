using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_contabilidad_partidas_detalle
    {
        [Key]
        public int PK_codigo { get; set; }
        public string numero_cuenta { get; set; }
        public string nombre_cuenta { get; set; }
        public string concepto { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public DateTime fecha { get; set; }
        public int FK_partida { get; set; }
        public int FK_cuenta_contable { get; set; }
        public int FK_mes { get; set; }
        public int FK_tipo { get; set; }
        public string cuenta_contable { get; set; }
        public string tipo { get; set; }
    }
}
