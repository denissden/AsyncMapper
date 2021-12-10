using AutoMapper;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public class AsyncMappingExpression<TSource, TDestination> : /*MappingExpression<TSource, TDestination>/*, IMappingExpression<TSource, TDestination>/,*/ IAsyncMappingExpression
    {   
        public IMappingExpression<TSource, TDestination> mappingExpression;

        public List<AsyncResolverConfig> _resolverConfigs { get; set; }

        // public AsyncMappingExpression(MemberList memberList) : base(memberList)
        // {
        // }

        // public AsyncMappingExpression() : this(new MemberList())
        // {
        // }

        public AsyncMappingExpression(IMappingExpression<TSource, TDestination> fromMap) //: base(new MemberList())
        {
            _resolverConfigs = new();
            mappingExpression = fromMap;
        }

        public AsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TResolver, TMember>(
            Expression<Func<TDestination, TMember>> destinationMember)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember> =>
            ForDestinationMember(destinationMember, o => o.AddResolver<TResolver>());


        public AsyncMappingExpression<TSource, TDestination> ForMember<TMember>(Expression<Func<TDestination, TMember>> destinationMember,
            Action<AsyncMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions) => 
            ForDestinationMember(destinationMember, memberOptions);


        public AsyncMappingExpression<TSource, TDestination> ForDestinationMember<TMember>(
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