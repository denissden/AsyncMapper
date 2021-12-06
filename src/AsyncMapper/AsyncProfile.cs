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
        public AsyncProfile() : base() 
        {
            AsyncMapConfig = new();
        }

        public Dictionary<TypePair, IAsyncMappingExpression> AsyncMapConfig { get; set; }

        public AsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {
            Console.WriteLine($"Create new async map: {typeof(TSource).Name} to {typeof(TDestination).Name}");
            AsyncMappingExpression<TSource, TDestination> expr = new(
                CreateMap<TSource, TDestination>()
            );
            AsyncMapConfig.Add(new TypePair(typeof(TSource), typeof(TDestination)), expr);
            return expr;
        }

        public IAsyncMappingExpression GetAsyncMapConfig(TypePair key) =>
            AsyncMapConfig.ContainsKey(key) ? AsyncMapConfig[key] : null;
    }
}
