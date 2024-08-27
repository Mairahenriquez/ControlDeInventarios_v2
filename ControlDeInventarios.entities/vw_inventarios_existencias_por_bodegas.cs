using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_inventarios_existencias_por_bodegas
    {
        [Key]
        public int PK_codigo { get; set; }
        public string identificador { get; set; }
        public string descripcion { get; set; }
        public decimal existencia_fisica { get; set; }
        public int FK_inventario { get; set; }
        public int FK_bodega { get; set; }
        public string bodega { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal? costo_total { get; set; }
        public decimal? precio_total { get; set; }
    }
}
