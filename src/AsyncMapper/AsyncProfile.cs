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
        public IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {
            return (IAsyncMappingExpression<TSource, TDestination>)CreateMap<TSource, TDestination>();
        }
    }
}
