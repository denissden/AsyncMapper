using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public class AsyncMappingExpression<TSource, TDestination> : IAsyncMappingExpression<TSource, TDestination>
    {
        private IMappingExpression<TSource, TDestination> _mappingExpression;
        public List<AsyncResolverConfig> _resolverConfigs { get; set; }
        public List<TypePair> _includedMaps { get; set; }
        public IMappingExpression<TSource, TDestination> Sync => _mappingExpression;

        public AsyncMappingExpression(IMappingExpression<TSource, TDestination> fromMap)
        {
            _resolverConfigs = new();
            _includedMaps = new();
            _mappingExpression = fromMap;
        }

        public IAsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TResolver, TMember>(
            Expression<Func<TDestination, TMember>> destinationMember)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember> =>
            ForDestinationMember(destinationMember, o => o.AddResolver<TResolver>());


        public IAsyncMappingExpression<TSource, TDestination> ForMember<TMember>(Expression<Func<TDestination, TMember>> destinationMember,
            Action<MemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions) => 
            ForDestinationMember(destinationMember, memberOptions);

        public IAsyncMappingExpression<TSource, TDestination> IncludeBase<TBaseSource, TBaseDestination>()
        {
            //throw new NotImplementedException();
            _mappingExpression.IncludeBase<TBaseSource, TBaseDestination>();
            _includedMaps.Add(new TypePair(typeof(TBaseSource), typeof(TBaseDestination)));
            return this;
        }

        IAsyncMappingExpression<TSource, TDestination> ForDestinationMember<TMember>(
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<MemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions)
        {
            var destinationInfo = AutoMapper.Internal.ReflectionHelper.FindProperty(destinationMember);
            //var sourceInfo = sourceMember != null ? ReflectionHelper.FindProperty(sourceMember) : null;
            //var sourceInfo = ReflectionHelper.FindProperty(sourceMember);

            var resolverConfig = new AsyncResolverConfig()
            {
                DestinationMemberInfo = destinationInfo,
            };
            var expr = new MemberConfigurationExpression<TSource, TDestination, TMember>(resolverConfig);

            memberOptions(expr);

            _resolverConfigs.Add(expr._config);
            // the property will be mapped by async mapper so ignore it
            _mappingExpression.ForMember(destinationMember, opt => opt.Ignore());

            return this;
        }

        public IMappingExpression<TSource, TDestination> EndAsyncConfig() => _mappingExpression;
    }
}