using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public class AsyncResolutionContext
    {
        AsyncMappingOptions Options { get; }
        private readonly IAsyncMapper _mapper;
        internal AsyncResolutionContext(AsyncMappingOptions options, IAsyncMapper mapper)
        {
            Options = options;
            _mapper = mapper;
        }

        internal AsyncResolutionContext(IAsyncMapper mapper) : this(default(AsyncMappingOptions), mapper) { }

        public object GetResolverInstance(Type type)
        {
            // if dependency injection is not used
            if (Options?.ServiceCtor == null) return Activator.CreateInstance(type);

            var argumentTypes = ReflectionHelper.GetConstructorArguments(type);
            var arguments = argumentTypes.Select(t => GetService(t)).ToArray();

            return Activator.CreateInstance(type, arguments);
        }

        internal object GetService(Type type)
        {
            var service = Options.ServiceCtor(type);
            if (service == null)
            {
                throw new NullReferenceException("Cannot create an instance of type " + type + " (service was null)");
            }
            return service;
        }
    }
}
