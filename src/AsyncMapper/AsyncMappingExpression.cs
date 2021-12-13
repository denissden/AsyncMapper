using AutoMapper;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public class AsyncMappingExpression<TSource, TDestination> : IAsyncMappingExpression
    {   
        public IMappingExpression<TSource, TDestination> mappingExpression;

        public List<AsyncResolverConfig> _resolverConfigs { get; set; }
        public List<TypePair> _includedMaps { get; set; }


        public AsyncMappingExpression(IMappingExpression<TSource, TDestination> fromMap) //: base(new MemberList())
        {
            _resolverConfigs = new();
            _includedMaps = new();
            mappingExpression = fromMap;
        }

        public AsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TResolver, TMember>(
            Expression<Func<TDestination, TMember>> destinationMember)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember> =>
            ForDestinationMember(destinationMember, o => o.AddResolver<TResolver>());


        public AsyncMappingExpression<TSource, TDestination> ForMember<TMember>(Expression<Func<TDestination, TMember>> destinationMember,
            Action<AsyncMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions) => 
            ForDestinationMember(destinationMember, memberOptions);

        public AsyncMappingExpression<TSource, TDestination> IncludeBase<TBaseSource, TBaseDestination>()
        {
            throw new NotImplementedException();
            mappingExpression.IncludeBase<TBaseSource, TBaseDestination>();
            _includedMaps.Add(new TypePair(typeof(TBaseSource), typeof(TBaseDestination)));
            return this;
        }


        AsyncMappingExpression<TSource, TDestination> ForDestinationMember<TMember>(
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<AsyncMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions)
        {
            var destinationInfo = AutoMapper.Internal.ReflectionHelper.FindProperty(destinationMember);
            //var sourceInfo = sourceMember != null ? ReflectionHelper.FindProperty(sourceMember) : null;
            //var sourceInfo = ReflectionHelper.FindProperty(sourceMember);

            var resolverConfig = new AsyncResolverConfig()
            {
                DestinationMemberInfo = destinationInfo,
            };
            var expr = new AsyncMemberConfigurationExpression<TSource, TDestination, TMember>(resolverConfig);

            memberOptions(expr);

            _resolverConfigs.Add(expr._config);
            // the property will be mapped by async mapper so ignore it
            mappingExpression.ForMember(destinationMember, opt => opt.Ignore());

            return this;
        }

        public IMappingExpression<TSource, TDestination> EndAsyncConfig() => mappingExpression;
    }
}