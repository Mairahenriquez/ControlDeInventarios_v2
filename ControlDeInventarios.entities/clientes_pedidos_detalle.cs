﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class clientes_pedidos_detalle
    {
        [Key]
        public int PK_codigo { get; set; }
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal subtotal { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public int FK_bodega { get; set; }
        public int FK_inventario { get; set; }
        public int FK_pedido { get; set; }
    }
}
