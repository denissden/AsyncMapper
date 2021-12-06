using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Collections.Generic;
using System;

namespace AsyncMapper
{

    public class AsyncMapperConfigurationExpression : MapperConfigurationExpression
    {
        public Dictionary<TypePair, IAsyncMappingExpression> AsyncMapConfig = new();
        public AsyncMapperConfigurationExpression() : base()
        {

        }

        public AsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {   
            Console.WriteLine($"Create new async map: {typeof(TSource).Name} to {typeof(TDestination).Name}");
            AsyncMappingExpression<TSource, TDestination> expr = new(
                CreateMap<TSource, TDestination>()
            );
            AsyncMapConfig.Add(new TypePair(typeof(TSource), typeof(TDestination)), expr);
            //expr.mappingExpression = CreateMap<TSource, TDestination>();
            return expr;
        } 

        public IAsyncMappingExpression GetAsyncMapConfig(TypePair key) => 
            AsyncMapConfig.ContainsKey(key) ? AsyncMapConfig[key] : null;
    }
}