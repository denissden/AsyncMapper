using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public interface ISyncConfig<T>
    {
        public abstract T Sync { get; }
    }
}
