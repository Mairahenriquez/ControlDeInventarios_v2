using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class clientes
    {
        [Key]
        public int PK_codigo { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string nit { get; set; }
        public string dui { get; set; }
        public string nrc { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public string giro { get; set; }
        public string nombre_comercial { get; set; }
        public string observaciones { get; set; }
        public string imagen { get; set; }
        public decimal abonos { get; set; }
        public decimal cargos { get; set; }
        public decimal total { get; set; }
        public int FK_municipio { get; set; }
        public int FK_estado { get; set; }
        public int FK_cuenta_contable { get; set; }
        public int FK_usuario { get; set; }
        public int FK_condicion_pago { get; set; }
    }
}
