using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;

namespace AsyncMapper
{
    // marks profile as an async profile
    public class AsyncProfile : Profile, MAsyncProfile
    {
        // reference an extension function so map can be created without "this":
        // CreateAsyncMap<...> not this.CreateAsyncMap<...>
        public IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>() =>
            (this as MAsyncProfile).CreateAsyncMap<TSource, TDestination>();
    }

    public static class AsyncProfileExtensions {

        public static IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>(this MAsyncProfile instance)
        {
            //Console.WriteLine($"Create new async map: {typeof(TSource).Name} to {typeof(TDestination).Name}");
            AsyncMappingExpression<TSource, TDestination> expr = new(
                    instance.CreateMappingExpression<TSource, TDestination>()
                );
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));
            instance.GetFields()._configuredAsyncMaps.Add(typePair, expr);
            return expr;
        }

        /// <summary>
        /// Since the class is inherited from <see cref="Profile"/>,
        /// create a map using base class function
        /// </summary>
        /// <returns>Mapping expression (syncronous)</returns>
        internal static IMappingExpression<TSource, TDestination> CreateMappingExpression<TSource, TDestination>(this MAsyncProfile instance)
        {
            return (instance as Profile).CreateMap<TSource, TDestination>();
        }

        public static IAsyncMappingExpression GetAsyncMapConfig<TAsyncProfile>(this TAsyncProfile instance, TypePair key) where TAsyncProfile : MAsyncProfile
        {
            var maps = instance.GetFields()._configuredAsyncMaps;
            return maps.ContainsKey(key) ? maps[key] : null;
        }
    }

    // marker interface
    public interface MAsyncProfile { } 

    // extra fields for AsyncProfile
    // this 
    public static class AsyncProfileProvider
    {
        static ConditionalWeakTable<MAsyncProfile, Fields> table = new();

        public class Fields
        {
            public List<AsyncProfile> _asyncProfiles = new();
            public Dictionary<TypePair, IAsyncMappingExpression> _configuredAsyncMaps { get; set; } = new();
        }

        public static Fields GetFields(this MAsyncProfile instance)
        {
            return table.GetOrCreateValue(instance);
        }
    }
}
