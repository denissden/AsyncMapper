using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;

namespace AsyncMapper
{

    public class AsyncMapperConfiguration : MapperConfiguration
    {   
        public readonly AsyncMapperConfigurationExpression _configurationProvider;
        public AsyncMapperConfiguration(AsyncMapperConfigurationExpression configurationExpression) :
            base((MapperConfigurationExpression)configurationExpression)
        {   
            _configurationProvider = configurationExpression;
            //conf = _configurationProvider.conf;
            Console.WriteLine($"Expression has the following maps: \n{String.Join( "\n", _configurationProvider.AsyncMapConfig)}");
        }

        public AsyncMapperConfiguration(Action<AsyncMapperConfigurationExpression> configure) : 
        this(Build(configure))
        {
        }

        static AsyncMapperConfigurationExpression Build(Action<AsyncMapperConfigurationExpression> configure)
        {
            var expr = new AsyncMapperConfigurationExpression();
            configure(expr);
            return expr;
        }

        public AsyncMapper CreateAsyncMapper()
        {   
            return new AsyncMapper(this);
        }

    }
}