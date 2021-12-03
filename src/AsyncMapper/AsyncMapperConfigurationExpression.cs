using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;

namespace AsyncMapper
{

    public class AsyncMapperConfigurationExpression : MapperConfigurationExpression, IAsyncMapperConfigurationExpression
    {

        public AsyncMapperConfigurationExpression() : base()
        {

        }

        public IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {
            return (IAsyncMappingExpression<TSource, TDestination>)CreateMap<TSource, TDestination>();
        } 
    }
}