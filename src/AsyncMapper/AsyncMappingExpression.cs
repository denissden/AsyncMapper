using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsyncMapper
{
    /// <summary>
    /// Extends AutoMapper map configuration by adding support for async resolvers
    /// </summary>
    public static class AsyncMappingExpressionExtensions
    {
        /// <summary>
        /// Add an asyncronous resolver for an individual member
        /// </summary>
        /// <typeparam name="TSource">Used for return type</typeparam>
        /// <typeparam name="TDestination">User for return type</typeparam>
        /// <typeparam name="TResolver">Asyncronous resolver type</typeparam>
        /// <typeparam name="TMember">Member type</typeparam>
        /// <param name="destinationMember">Destination member getter expression</param>
        /// <returns>this</returns>
        public static IMappingExpression<TSource, TDestination> AddAsyncResolver<TSource, TDestination, TResolver, TMember>(
            this IMappingExpression<TSource, TDestination> instance,
            Expression<Func<TDestination, TMember>> destinationMember)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember> =>
            instance.ForDestinationMember(destinationMember, o => o.AddResolver<TResolver>());

        /// <summary>
        /// Configure individual members
        /// </summary>
        /// <typeparam name="TSource">Used for return type</typeparam>
        /// <typeparam name="TDestination">User for return type</typeparam>
        /// <typeparam name="TMember">Member type</typeparam>
        /// <param name="destinationMember">Destination member getter expression</param>
        /// <param name="memberOptions"></param>
        /// <returns>this</returns>
        public static IMappingExpression<TSource, TDestination> ForMemberAsync<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> instance,
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<MemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions) =>
            instance.ForDestinationMember(destinationMember, memberOptions);

        /// <summary>
        /// Include other map configuration
        /// </summary>
        /// <typeparam name="TSource">Used for return type</typeparam>
        /// <typeparam name="TDestination">User for return type</typeparam>
        /// <typeparam name="TBaseSource">Base source type</typeparam>
        /// <typeparam name="TBaseDestination">Base destination type</typeparam>
        /// <returns>this</returns>
        public static IMappingExpression<TSource, TDestination> IncludeBase<TSource, TDestination, TBaseSource, TBaseDestination>(
            this IMappingExpression<TSource, TDestination> instance)
        {

            //throw new NotImplementedException();
            instance.IncludeBase<TBaseSource, TBaseDestination>();
            instance.GetFields()._includedMaps.Add(new TypePair(typeof(TBaseSource), typeof(TBaseDestination)));
            return instance;
        }

        /// <summary>
        /// Confugure mapping that does not require asyncronous execution
        /// </summary>
        /// <returns>this</returns>
        public static IMappingExpression<TSource, TDestination> EndAsyncConfig<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> instance) => instance;

        /// <summary>
        /// Adds member configuration to map configuration
        /// </summary>
        /// <returns>this</returns>
        internal static IMappingExpression<TSource, TDestination> ForDestinationMember<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> instance,
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<MemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions)
        {
            var destinationInfo = AutoMapper.Internal.ReflectionHelper.FindProperty(destinationMember);
            var resolverConfig = new AsyncResolverConfig()
            {
                DestinationMemberInfo = destinationInfo,
            };
            var expr = new MemberConfigurationExpression<TSource, TDestination, TMember>(resolverConfig);
            memberOptions(expr);

            var fields = instance.GetFields();
            fields._resolverConfigs.Add(expr._config);
            // the property will be mapped by async mapper so ignore it
            instance.ForMember(destinationMember, opt => opt.Ignore());

            return instance;
        }
    }

    //public interface ITypeMapConfiguration : AutoMapper.Configuration.ITypeMapConfiguration { }

    /// <summary>
    /// Provides extra fields needed for <see cref="AsyncMappingExpressionExtensions"/>
    /// </summary>
    public static class AsyncMappingExpressionProvider
    {
        static ConditionalWeakTable<ITypeMapConfiguration, Fields> table = new();

        public class Fields
        {
            public List<AsyncResolverConfig> _resolverConfigs { get; set; } = new();
            public List<TypePair> _includedMaps { get; set; } = new();
        }

        internal static void MakeAsync(this ITypeMapConfiguration instance) => table.GetOrCreateValue(instance);

        public static Fields GetFields<TSource, TDestination>(this IMappingExpression<TSource, TDestination> instance) =>
            (instance as ITypeMapConfiguration).GetFields();

        public static Fields GetFields(this ITypeMapConfiguration instance)
        {
            if (table.TryGetValue(instance, out var fields))
                return fields;
            else
                throw new InvalidOperationException("The map is not marked async. Please change CreateMap to CreateAsyncMap");
        }
    }
}