using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;


namespace AsyncMapper
{
    public class MemberConfigurationExpression<TSource, TDestination, TMember> : IAsyncMemberConfigurationExpression<TSource, TDestination, TMember>
    {
        public AsyncResolverConfig _config { get; set; }

        public MemberConfigurationExpression() : this(new AsyncResolverConfig()) { }

        public MemberConfigurationExpression(AsyncResolverConfig config)
        {
            _config = config;
        }

        public void AddResolver<TResolver>()
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>
        {
            _config.ResolverType = typeof(TResolver);
            _config.MemberType = typeof(TMember);
            _config.MemberTaskType = typeof(Task<TMember>);
        }

        public void AddMemberResolver<TMemberResolver, TSourceMember>(Expression<Func<TSource, TSourceMember>> sourceMember)
            where TMemberResolver : IAsyncMemberValueResolver<TSource, TDestination, TSourceMember, TMember>
        {
            _config.SourceMemberGetter = sourceMember.Compile();
            _config.ResolverType = typeof(TMemberResolver);
            _config.MemberType = typeof(TMember);
            _config.MemberTaskType = typeof(Task<TMember>);
        }
    }
}
