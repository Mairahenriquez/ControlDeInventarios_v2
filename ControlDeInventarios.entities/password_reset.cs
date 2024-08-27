using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities
{
    // columns [PK_codigo] ,[nombre]

    public class password_reset
    {   
        // int PK_codigo
        [Key]
        public int PK_codigo { get; set; }
        // int FK_usuario 
        public int FK_usuario { get; set; }
        // varchar(max) hash
        public string hash { get; set; }
        // timestamp vence
        public DateTime vence { get; set; }

    }
}
