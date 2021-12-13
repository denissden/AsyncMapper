using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.UnitTests
{
    public abstract class TestBase
    {
        protected abstract AsyncMapperConfiguration Configuration { get; }


        public Dictionary<TypePair, IAsyncMappingExpression> GetMaps() => Configuration._configuredAsyncMaps;
    }
}
