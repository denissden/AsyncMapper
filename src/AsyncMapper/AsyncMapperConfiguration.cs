using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System;

namespace AsyncMapper
{

    public class AsyncMapperConfiguration : MapperConfiguration
    {
        public AsyncMapperConfiguration(AsyncMapperConfigurationExpression configurationExpression) :
            base((MapperConfigurationExpression)configurationExpression)
        {
        }

        public AsyncMapperConfiguration(Action<IAsyncMapperConfigurationExpression> configure) : base((Action<IMapperConfigurationExpression>)configure)
        {
            
        }
    }
}