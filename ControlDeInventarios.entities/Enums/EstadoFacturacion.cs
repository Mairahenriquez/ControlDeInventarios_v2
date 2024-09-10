using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeInventarios.entities.Enums
{
    enum EstadoFacturacion:int
    {
        Nueva=1,
	    Procesada=2,
	    Anulada=3,
	    Finalizada=4
    }
}
