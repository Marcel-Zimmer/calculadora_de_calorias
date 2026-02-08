using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraCalorias.Core.Domain.Common
{
    public enum TipoDeErro
    {
        None,
        NotFound,
        Validation,
        Conflict,    
        Unauthorized,
        SystemFailure 
    }
}
