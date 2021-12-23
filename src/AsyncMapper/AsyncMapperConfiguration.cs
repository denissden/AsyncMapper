using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;

namespace AsyncMapper
{
    /// <summary>
    /// Mapper Configuration 
    /// Here you can add maps and profiles
    /// </summary>
    public class AsyncMapperConfiguration : MapperConfiguration
    {   
        
        public readonly AsyncMapperConfigurationExpression _configurationProvider;
        public Dictionary<TypePair, IAsyncMappingExpression> _configuredAsyncMaps { get; set; }
        

        public AsyncMapperConfiguration(AsyncMapperConfigurationExpression configurationExpression) :
            base(configurationExpression.Sync)
        {  
            _configurationProvider = configurationExpression;
            _configuredAsyncMaps = configurationExpression._configuredAsyncMaps;
            var asyncProfiles = configurationExpression._asyncProfiles;
            foreach (var p in asyncProfiles)
            {
                foreach (var m in p._configuredAsyncMaps)
                {
                    _configuredAsyncMaps.Add(m.Key, m.Value);
                }
            }
            //conf = _configurationProvider.conf;
            Console.WriteLine($"Expression has the following maps: \n{String.Join( "\n", _configurationProvider._configuredAsyncMaps)}");
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

        public Mapper CreateAsyncMapper()
        {   
            return new Mapper(this);
        }


        public IAsyncMappingExpression GetAsyncMapConfig(TypePair key) => 
            _configuredAsyncMaps.ContainsKey(key) ? _configuredAsyncMaps[key] : null;
    }
}