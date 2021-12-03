using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;

namespace AsyncMapper;

public class AsyncMapperConfiguration : MapperConfiguration
{
    public AsyncMapperConfiguration(AsyncMapperConfigurationExpression configurationExpression) :
        base((MapperConfigurationExpression)configurationExpression)
    {   
    }

    public AsyncMapperConfiguration(Action<IMapperConfigurationExpression> configure) : base(configure)
    {
    }
}