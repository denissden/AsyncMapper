using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Exceptions
{
    public class MapperConfigurationException : Exception
    {
        public MapperConfigurationException(string message) : base(message)
        { }
    }
}
