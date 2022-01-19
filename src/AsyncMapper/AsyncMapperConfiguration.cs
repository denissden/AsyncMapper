using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using AsyncMapper.Exceptions;
using System.Linq;

namespace AsyncMapper
{
    /// <summary>
    /// Mapper Configuration 
    /// Here you can add maps and profiles
    /// </summary>
    public class AsyncMapperConfiguration : MapperConfiguration
    {   
        
        public readonly AsyncProfileProvider.Fields _configurationProvider;
        public Dictionary<TypePair, AsyncMappingExpressionProvider.Fields> _configuredAsyncMaps { get; set; } = new();
        
        /// <summary>
        /// Compiles all map and profile configurations into one configuration that is used by mapper
        /// </summary>
        /// <param name="configurationExpression">Configuration expression</param>
        public AsyncMapperConfiguration(AsyncMapperConfigurationExpression configurationExpression) :
            base(configurationExpression)
        {  
            _configurationProvider = configurationExpression.GetFields();
            foreach (var kv in _configurationProvider._configuredAsyncMaps)
            {
                _configuredAsyncMaps.Add(kv.Key, kv.Value.GetFields());
            }
            var asyncProfiles = _configurationProvider._asyncProfiles;
            foreach (var p in asyncProfiles)
            {
                foreach (var m in p.GetFields()._configuredAsyncMaps)
                {
                    _configuredAsyncMaps.Add(m.Key, m.Value.GetFields());
                }
            }
            // IncludeBase
            // uses base member configuration if it is not overriden
            foreach (var map in _configuredAsyncMaps.Values)
            {
                CompileIncludeBase(map);
            }
            //conf = _configurationProvider.conf;
            Console.WriteLine($"Expression has the following maps: \n{String.Join( "\n", _configuredAsyncMaps)}");
        }

        private void CompileIncludeBase(AsyncMappingExpressionProvider.Fields map)
        {
            // add map child map configuration first
            // so already existing base configs are not added
            HashSet<AsyncResolverConfig> asyncResolverConfigs = new(map._resolverConfigs);
            foreach (var baseMapTypePair in map._includedMaps)
            {
                var baseMap = GetAsyncMapConfig(baseMapTypePair) ??
                    throw new MapperConfigurationException(
                        $"Error including base map [{baseMapTypePair.SourceType.Name} to" +
                        $" {baseMapTypePair.DestinationType.Name}]: \n\tmap does not exist!"
                        );
                CompileIncludeBase(baseMap);
                foreach (var conf in baseMap._resolverConfigs)
                {   
                    // only adds if member configuration does not already exist
                    asyncResolverConfigs.Add(conf);
                }
                map._resolverConfigs = asyncResolverConfigs.ToList();
            }
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


        public AsyncMappingExpressionProvider.Fields GetAsyncMapConfig(TypePair key) => 
            _configuredAsyncMaps.ContainsKey(key) ? _configuredAsyncMaps[key] : null;
    }
}