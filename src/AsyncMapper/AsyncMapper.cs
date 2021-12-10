using AutoMapper;
using System;
using System.Runtime;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace AsyncMapper
{
    /// <summary>
    /// Mapper
    /// Configured with AsyncMapperConfiguration
    /// </summary>
    public class AsyncMapper : IAsyncMapper
    {

        public IMapper _mapper { get; set; }
        AsyncMapperConfiguration _configurationProvider;
        public AsyncMapper(IConfigurationProvider configurationProvider)
        {
            var asyncConf = (AsyncMapperConfiguration)configurationProvider ??
             throw new ArgumentNullException(nameof(configurationProvider));
            _configurationProvider = asyncConf;
            _mapper = new Mapper(configurationProvider);
        }

        public AsyncMapper(IConfigurationProvider configurationProvider, IMapper mapper)
        {
            var asyncConf = (AsyncMapperConfiguration)configurationProvider ??
             throw new ArgumentNullException(nameof(configurationProvider));
            _configurationProvider = asyncConf;
            _mapper = mapper;
        }

        public async Task<TDestination> Map<TDestination>(object source) => await Map(source, default(TDestination));
        public async Task<TDestination> Map<TSource, TDestination>(TSource source) => await Map(source, default(TDestination));
        public async Task<TDestination> Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            destination = destination ?? Activator.CreateInstance<TDestination>();
            Console.WriteLine($"Mapping types: {source.GetType()} to {destination.GetType()}");

            Console.WriteLine($"Passed types: {typeof(TSource)} to {typeof(TDestination)}");
            var mapTypePair = new TypePair(source.GetType(), typeof(TDestination));
            var map = _configurationProvider
                .GetAsyncMapConfig(mapTypePair);
            Console.WriteLine($"Config of map: {map?._resolverConfigs}");


            List<Task> asyncResolverTaks = new();
            // add tasks for mapping all async resolvers
            foreach (var conf in map?._resolverConfigs)
            {
                Console.WriteLine($"Property {conf} of {typeof(TDestination)} is {ReflectionHelper.GetMemberValue(conf.DestinationMemberInfo, destination)}");
                var resolverInstance = Activator.CreateInstance(conf.ResolverType);
                var resolveMethod = conf.ResolverType.GetMethod("Resolve");

                var valueAwaiter = (dynamic)(resolverInstance switch
                {
                    IAsyncValueResolver => resolveMethod.Invoke(resolverInstance, new object[] { source, destination }),
                    IAsyncMemberValueResolver => resolveMethod.Invoke(resolverInstance, new object[]
                        {
                            source,
                            destination,
                            ReflectionHelper.GetMemberValue(conf.SourceMemberInfo, source)
                        }),
                    _ => Task.FromResult(0),
                });

                async Task TaskRunner()
                {
                    var value = await valueAwaiter;
                    ReflectionHelper.SetMemberValue(conf.DestinationMemberInfo, destination, value);
                    Console.WriteLine($"Finish running resolver {conf.ResolverType.Name}");
                }
                Console.WriteLine($"Add task resolver {conf.ResolverType.Name}");
                asyncResolverTaks.Add(TaskRunner());
                /*asyncResolverTaks.Add(async () =>
                {
                    var value = await valueAwaiter;
                    SetMemberValue(conf.DestinationMemberInfo, destination, value);
                    Console.WriteLine($"Finish running resolver {conf.ResolverType.Name}");
                });*/
            }

            // add a task for mapping done by automapper
            int RunMap()
            {
                Console.WriteLine("Start map");
                _mapper.Map<TSource, TDestination>(source, destination);
                Console.WriteLine("End map");
                return 0;
            }
            // runs in the same thread
            //asyncResolverTaks.Add(Task.FromResult(RunMap()));
            // runs in a different thread
            //asyncResolverTaks.Add(Task.Run(() => mapper.Map<TSource, TDestination>(source, destination)));
            // runs in current thread, synchronously
            // execution time is still the same, because resolvers are started in the loop before map
            _mapper.Map<TSource, TDestination>(source, destination);
            await Task.WhenAll(asyncResolverTaks);
            return destination;
        }

        public static Expression ToType(Expression expression, Type type) => expression.Type == type ? expression : Expression.Convert(expression, type);
    }
}