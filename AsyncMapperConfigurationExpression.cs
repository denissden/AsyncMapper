using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;

namespace AsyncMapper;

public class AsyncMapperConfigurationExpression : MapperConfigurationExpression
{

    public AsyncMapperConfigurationExpression() : base()
    {
        
    }

    public IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
    {
        return (IAsyncMappingExpression<TSource, TDestination>)CreateMap<TSource, TDestination>();
    } I
    
}