using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_inventarios_kardex_ventas
    {
        [Key]
        public int PK_codigo { get; set; }
        public string identificador { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal precio_unitario { get; set; }
        public string estado { get; set; }
        public string estado_color { get; set; }
        public string descripcion { get; set; }
        public decimal existencia_fisica { get; set; }
    }
}
