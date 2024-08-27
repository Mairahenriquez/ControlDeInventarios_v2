using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_bodegas
    {
        [Key]
        public int PK_codigo { get; set; }
        public string identificador { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string observaciones { get; set; }
        public decimal cantidad_total { get; set; }
        public decimal costo_total { get; set; }
        public decimal precio_total { get; set; }
        public int FK_usuario { get; set; }
        public int FK_estado { get; set; }
        public string usuario { get; set; }
        public string estado { get; set; }
        public string estado_color { get; set; }
    }
}
