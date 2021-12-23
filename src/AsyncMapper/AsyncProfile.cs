using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;

namespace AsyncMapper
{
    public class AsyncProfile : Profile, IAsyncMapperConfigurationExpression, ISyncConfig<Profile>
    {
        public Dictionary<TypePair, IAsyncMappingExpression> _configuredAsyncMaps { get; set; }

        public virtual Profile Sync => this;

        protected AsyncProfile() : base() 
        {
            _configuredAsyncMaps = new();
        }

        public IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {
            //Console.WriteLine($"Create new async map: {typeof(TSource).Name} to {typeof(TDestination).Name}");
            AsyncMappingExpression<TSource, TDestination> expr = new(
                CreateMappingExpression<TSource, TDestination>()
            );
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));
            _configuredAsyncMaps.Add(typePair, expr);
            return expr;
        }

        /// <summary>
        /// Since the class is inherited from <see cref="Profile"/>,
        /// create a map using base class function
        /// </summary>
        /// <returns>Mapping expression (syncronous)</returns>
        internal virtual IMappingExpression<TSource, TDestination> CreateMappingExpression<TSource, TDestination>()
        {
            return CreateMap<TSource, TDestination>();
        }

        public IAsyncMappingExpression GetAsyncMapConfig(TypePair key) =>
            _configuredAsyncMaps.ContainsKey(key) ? _configuredAsyncMaps[key] : null;
    }
}
