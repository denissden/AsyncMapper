using AutoMapper;
using System;
using System.Runtime;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using AsyncMapper.Exceptions;

namespace AsyncMapper
{
    /// <summary>
    /// Mapper
    /// Configured with AsyncMapperConfiguration
    /// </summary>
    public class Mapper : IAsyncMapper
    {

        private AutoMapper.IMapper _mapper;
        public IMapper Sync => _mapper;

        AsyncMapperConfiguration _configurationProvider;
        AsyncResolutionContext _context;
        

        public Mapper(IConfigurationProvider configurationProvider)
        {
            var asyncConf = (AsyncMapperConfiguration)configurationProvider ??
             throw new ArgumentNullException(nameof(configurationProvider));
            _configurationProvider = asyncConf;
            _mapper = new AutoMapper.Mapper(configurationProvider);
            _context = new AsyncResolutionContext(this);
        }

        public Mapper(IConfigurationProvider configurationProvider, Func<Type, object> serviceCtor) : this(configurationProvider)
        {
            _context = new AsyncResolutionContext(new AsyncMappingOptions(serviceCtor ?? throw new NullReferenceException(nameof(serviceCtor))) ,this);
        }

        public Mapper(IConfigurationProvider configurationProvider, IMapper mapper) : this(configurationProvider)
        {
            _mapper = mapper;
        }

        public async Task<TDestination> Map<TDestination>(object source) => await Map(source, default(TDestination));
        public async Task<TDestination> Map<TSource, TDestination>(TSource source) => await Map(source, default(TDestination));
        public async Task<TDestination> Map<TSource, TDestination>(TSource source, TDestination destination)
        {   
            // create destination instance if needed
            destination = destination ?? Activator.CreateInstance<TDestination>();

            // find the map in configuration
            var mapTypePair = new TypePair(source.GetType(), typeof(TDestination));
            var map = _configurationProvider
                .GetAsyncMapConfig(mapTypePair) ??
                null; //throw new MappingException(mapTypePair, "The async map does not exist.");

            
            List<Task> asyncResolverTasks = new();
            
            // if async map is not configured, skip to synchronous mapping 
            if (map != null)
            foreach (var conf in map._resolverConfigs)
            {
                var resolverInstance = _context.GetResolverInstance(conf.ResolverType);
                var resolveMethod = conf.ResolverType.GetMethod("Resolve");

                // get value awaiter based on resolver type
                var valueAwaiter = (dynamic)(resolverInstance switch
                {   
                    // value resolver takes 2 arguments
                    IAsyncValueResolver => resolveMethod.Invoke(resolverInstance, new object[] { source, destination }),
                    // member value resolver takes 3 arguments
                    IAsyncMemberValueResolver => resolveMethod.Invoke(resolverInstance, new object[]
                        {
                            source,
                            destination,
                            ReflectionHelper.GetMemberValue(conf.SourceMemberInfo, source)
                        }),
                    // default case
                    _ => Task.FromResult(0),
                });

                // create resolve task
                async Task TaskRunner()
                {
                    var value = await valueAwaiter;
                    ReflectionHelper.SetMemberValue(conf.DestinationMemberInfo, destination, value);
                }
                // run task and add awaiter to the list
                asyncResolverTasks.Add(TaskRunner());
            }

            // call synchronous mapper and wait for all resolve tasks to finish
            _mapper.Map(source, destination, mapTypePair.SourceType, mapTypePair.DestinationType);
            await Task.WhenAll(asyncResolverTasks);
            return destination;
        }

        public static Expression ToType(Expression expression, Type type) => expression.Type == type ? expression : Expression.Convert(expression, type);
    }
}