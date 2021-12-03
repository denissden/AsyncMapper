using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace AsyncMapper
{
    public class AsyncProfile : Profile
    {  

        public AsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {   
            AsyncMappingExpression<TSource, TDestination> expr = new();
            expr.mappingExpression = CreateMap<TSource, TDestination>();
            return expr;
        }
    }
}
