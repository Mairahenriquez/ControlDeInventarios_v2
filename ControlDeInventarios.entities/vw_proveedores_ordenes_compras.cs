using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class vw_proveedores_ordenes_compras
    {
        [Key]
        public int PK_codigo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fecha_hora { get; set; }
        public string observaciones { get; set; }
        public decimal subtotal { get; set; }
        public decimal iva { get; set; }
        public decimal descuento { get; set; }
        public decimal total { get; set; }
        public int FK_bodega { get; set; }
        public int FK_proveedor { get; set; }
        public int FK_forma_pago { get; set; }
        public int? FK_partida { get; set; }
        public int FK_estado { get; set; }
        public int FK_usuario { get; set; }
        public int FK_condicion_pago { get; set; }
        public int? FK_movimiento { get; set; }
        public string bodega { get; set; }
        public string proveedor { get; set; }
        public string forma_pago { get; set; }
        public string partida { get; set; }
        public string condicion_pago { get; set; }
        public string estado { get; set; }
        public string estado_color { get; set; }
    }
}
