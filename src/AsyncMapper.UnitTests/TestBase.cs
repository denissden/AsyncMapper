using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.UnitTests
{   
    public class TestBase : IDisposable
    {
        protected TestBase()
        {
            Initialize();
        }

        protected virtual void Initialize() { }

        public void Dispose()
        {
            // do nothing
        }
    }

    public abstract class TestConfigurationBase : TestBase
    {
        protected abstract AsyncMapperConfiguration Configuration { get; }
        public Dictionary<TypePair, AsyncMappingExpressionProvider.Fields> GetMaps() => Configuration._configuredAsyncMaps;

    }
}
