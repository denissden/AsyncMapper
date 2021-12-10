using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace AsyncMapper
{
    public class AsyncProfile : Profile, IAsyncMapperConfigurationExpression
    {
        public Dictionary<TypePair, IAsyncMappingExpression> _configuredAsyncMaps { get; set; }

        public AsyncProfile() : base() 
        {
            _configuredAsyncMaps = new();
        }

        public AsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {
            Console.WriteLine($"Create new async map: {typeof(TSource).Name} to {typeof(TDestination).Name}");
            AsyncMappingExpression<TSource, TDestination> expr = new(
                CreateMap<TSource, TDestination>()
            );
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));
            //if (_configuredAsyncMaps.ContainsKey(typePair))
            //    throw new ArgumentException($"Map from {typeof(TSource).Name} to {typeof(TDestination).Name} has already been configured.");
            _configuredAsyncMaps.Add(typePair, expr);
            return expr;
        }

        public IAsyncMappingExpression GetAsyncMapConfig(TypePair key) =>
            _configuredAsyncMaps.ContainsKey(key) ? _configuredAsyncMaps[key] : null;
    }
}
