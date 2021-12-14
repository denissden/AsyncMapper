using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AsyncMapper
{
    public interface IAsyncMappingExpression<TSource, TDestination> : IMappingExpression<TSource, TDestination>
    {
        public IAsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TMember, TResolver>(
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<IMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>;

        public IAsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TMember, TResolver>(
            Expression<Func<TDestination, TMember>> destinationMember)
                where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>;
        
    }

    public interface IAsyncMappingExpression
    {
        public List<TypePair> _includedMaps { get; set; }
        public List<AsyncResolverConfig> _resolverConfigs { get; set; }
    }
}