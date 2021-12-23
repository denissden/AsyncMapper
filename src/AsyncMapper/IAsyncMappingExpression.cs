using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AsyncMapper
{
    public interface IAsyncMappingExpression<TSource, TDestination> : IAsyncMappingExpression,
        ISyncConfig<IMappingExpression<TSource, TDestination>>
    {
        /// <summary>
        /// Confugure mapping that does not require asyncronous execution
        /// </summary>
        /// <returns>AutoMapper configuration expresion</returns>
        IMappingExpression<TSource, TDestination> EndAsyncConfig();


        /// <summary>
        /// Add an asyncronous resolver for an individual member
        /// </summary>
        /// <typeparam name="TResolver">Asyncronous resolver type</typeparam>
        /// <typeparam name="TMember">Member type</typeparam>
        /// <param name="destinationMember">Destination member getter expression</param>
        /// <returns>Itself</returns>
        public IAsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TResolver, TMember>(
            Expression<Func<TDestination, TMember>> destinationMember)
                where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>;
        /// <summary>
        /// Configure individual members
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destinationMember">Destination member getter expression</param>
        /// <param name="memberOptions"></param>
        /// <returns>Itself</returns>
        public IAsyncMappingExpression<TSource, TDestination> ForMember<TMember>(Expression<Func<TDestination, TMember>> destinationMember,
            Action<MemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions);
        /// <summary>
        /// Include other map configuration
        /// </summary>
        /// <typeparam name="TBaseSource">Base source type</typeparam>
        /// <typeparam name="TBaseDestination">Base destination type</typeparam>
        /// <returns>Itself</returns>
        IAsyncMappingExpression<TSource, TDestination> IncludeBase<TBaseSource, TBaseDestination>();
    }

    public interface IAsyncMappingExpression
    {
        public List<TypePair> _includedMaps { get; set; }
        public List<AsyncResolverConfig> _resolverConfigs { get; set; }
    }
}