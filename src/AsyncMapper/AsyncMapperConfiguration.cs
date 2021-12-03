using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;

namespace AsyncMapper
{

    public class AsyncMapperConfiguration : MapperConfiguration
    {
        public List<string> conf = new List<string>();
        public AsyncMapperConfiguration(AsyncMapperConfigurationExpression configurationExpression) :
            base((MapperConfigurationExpression)configurationExpression)
        {
            conf = configurationExpression.conf;
            Console.WriteLine($"Expression has the following maps: \n{String.Join( "\n", configurationExpression.conf)}");
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
            Console.WriteLine($"Creating async mapper with maps: \n{String.Join( "\n", conf)}");
            return new AsyncMapper(this);
        }
        
    }
}