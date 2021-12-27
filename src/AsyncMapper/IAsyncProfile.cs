using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace AsyncMapper
{
    public interface IAsyncProfile
    {
        Dictionary<TypePair, IAsyncMappingExpression> _configuredAsyncMaps { get; set; }

        /// <summary>
        /// Creates a mapping configuration from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <returns>Expression to further configure the map</returns>
        IAsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>();
        IAsyncMappingExpression GetAsyncMapConfig(TypePair key);
    }
}