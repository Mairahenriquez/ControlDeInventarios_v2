using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class condiciones_pagos
    {
        [Key]
        public int PK_codigo { get; set; }
        public string nombre { get; set; }
        public string dias { get; set; }
    }
}
