﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    public class contabilidad_cuentas_contables_clasificaciones
    {
        [Key]
        public int PK_codigo { get; set; }
        public string identificador { get; set; }
        public string nombre { get; set; }
    }
}
