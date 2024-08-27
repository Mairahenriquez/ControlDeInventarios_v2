using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class contabilidad_partidas
    {
        [Key]
        public int PK_codigo { get; set; }
        public string concepto { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fecha_hora { get; set; }
        public string periodo { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public DateTime? fecha_procesado { get; set; }
        public int FK_estado { get; set; }
        public int FK_tipo_partida { get; set; }
        public int FK_mes { get; set; }
        public int FK_usuario { get; set; }
        public int? FK_compra { get; set; }
        public int? FK_factura { get; set; }
    }
}
