using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Collections.Generic;
using System;

namespace AsyncMapper
{

    public class AsyncMapperConfigurationExpression : MapperConfigurationExpression
    {
        public List<string> conf = new List<string>();
        public AsyncMapperConfigurationExpression() : base()
        {

        }

        public AsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {   
            var add = $"{typeof(TSource).Name} to {typeof(TDestination).Name}";
            conf.Add(add);
            Console.WriteLine("Added a new map: " + add);
            AsyncMappingExpression<TSource, TDestination> expr = new();
            expr.mappingExpression = CreateMap<TSource, TDestination>();
            return expr;
        } 
    }
}