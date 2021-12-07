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

    public class AsyncMapper
    {   

        public Mapper mapper;
        AsyncMapperConfiguration _configurationProvider;
        public AsyncMapper(IConfigurationProvider configurationProvider)
        {   
            var asyncConf = (AsyncMapperConfiguration)configurationProvider ??
             throw new ArgumentNullException(nameof(configurationProvider));
            _configurationProvider = asyncConf;
            mapper = new Mapper(configurationProvider);
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
            Console.WriteLine($"Config of map: {map.conf}"); 
            var endMap = mapper.Map<TSource, TDestination>(source, destination);

            List<Task> asyncResolverTaks = new();
            foreach (var conf in map.conf) {
                Console.WriteLine($"Property {conf} of {typeof(TDestination)} is {conf.MemberGetter.DynamicInvoke(destination)}");
                var resolverInstance = Activator.CreateInstance(conf.ResolverType);
                //var resolver = Expression.Constant(resolverInstance);
                var resolveMethod = conf.ResolverType.GetMethod("Resolve");
                var valueAwaiter = (dynamic) resolveMethod.Invoke(resolverInstance, new[] { source, destination, conf.MemberGetter.DynamicInvoke(destination) });
                //var value = await valueAwaiter;
                async Task TaskRunner()
                {
                    var value = await valueAwaiter;
                    SetMemberValue(conf.ToMemberInfo, destination, value);
                }
                asyncResolverTaks.Add(TaskRunner());
                //Expression expSource = () => source;
                //var value = Expression.Call(ToType(resolver, conf.ResolverType), resolveMethod, () => source, destination, conf.MemberGetter);
                //SetMemberValue(conf.ToMemberInfo, destination, value);
            }
            await Task.WhenAll(asyncResolverTaks);
            return endMap;
        }

        public static void SetMemberValue(MemberInfo memberInfo, object target, object value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)memberInfo).SetValue(target, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)memberInfo).SetValue(target, value);
                    break;
                default:
                    throw new ArgumentException("MemberInfo must be FieldInfo or PropertyInfo");
            }
        }

        public static Expression ToType(Expression expression, Type type) => expression.Type == type ? expression : Expression.Convert(expression, type);

    }
}