using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace AsyncMapper
{
    public interface IAsyncMapperConfigurationExpression
    {
        public Dictionary<TypePair, IAsyncMappingExpression> _configuredAsyncMaps { get; set; }
        public AsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>();
    }
}