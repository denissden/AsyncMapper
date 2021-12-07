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
            _configuredAsyncMaps.Add(new TypePair(typeof(TSource), typeof(TDestination)), expr);
            return expr;
        }

        public IAsyncMappingExpression GetAsyncMapConfig(TypePair key) =>
            _configuredAsyncMaps.ContainsKey(key) ? _configuredAsyncMaps[key] : null;
    }
}
