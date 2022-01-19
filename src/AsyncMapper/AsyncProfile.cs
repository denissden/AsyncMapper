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
    /// <summary>
    /// Marks profile Async
    /// </summary>
    public class AsyncProfile : Profile, MAsyncProfile
    {
        /// <inheritdoc cref="AsyncProfileExtensions.CreateAsyncMap{TSource, TDestination}(MAsyncProfile)"/>
        public IMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>() =>
            // convert to marker interface so the function doesn't call itself
            (this as MAsyncProfile).CreateAsyncMap<TSource, TDestination>();
    }

    /// <summary>
    /// Extensions for <see cref="AsyncProfile"/>
    /// Implemented via extensions so <see cref="AsyncProfileProvider.Fields"/> can be inherited by <see cref="AsyncMapperConfigurationExpression"/>
    /// </summary>
    public static class AsyncProfileExtensions {

        /// <summary>
        /// Creates a mapping configuration from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <returns>Expression to further configure the map</returns>
        public static IMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>(this MAsyncProfile instance)
        {
            var expr = instance.CreateMappingExpression<TSource, TDestination>();
            var abstractExpr = expr as ITypeMapConfiguration;
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));
            instance.GetFields()._configuredAsyncMaps.Add(typePair, abstractExpr);
            abstractExpr.MakeAsync();
            return expr;
        }

        /// <summary>
        /// Create synchronous map to be later used by <see cref="Mapper"/>
        /// </summary>
        /// <returns>Mapping expression (synchronous)</returns>
        internal static IMappingExpression<TSource, TDestination> CreateMappingExpression<TSource, TDestination>(this MAsyncProfile instance)
        {
            return (instance as Profile).CreateMap<TSource, TDestination>();
        }
    }

    /// <summary>
    /// Marker
    /// </summary>
    public interface MAsyncProfile { }

    /// <summary>
    /// Provides extra fields needed for <see cref="AsyncProfileExtensions"/> and <see cref="AsyncMapperConfigurationExpression"/>
    /// </summary>
    public static class AsyncProfileProvider
    {
        static ConditionalWeakTable<MAsyncProfile, Fields> table = new();

        public class Fields
        {
            public List<AsyncProfile> _asyncProfiles = new();
            public Dictionary<TypePair, ITypeMapConfiguration> _configuredAsyncMaps { get; set; } = new();
        }

        public static Fields GetFields(this MAsyncProfile instance)
        {
            return table.GetOrCreateValue(instance);
        }
    }
}
