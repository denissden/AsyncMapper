using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public class AsyncMappingOptions
    {
        public Func<Type, object> ServiceCtor { get; private set; } 
        public AsyncMappingOptions(Func<Type, object> serviceCtor) => ServiceCtor = serviceCtor;
    }
}
