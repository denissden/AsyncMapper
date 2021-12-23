using System;
using System.Linq.Expressions;

namespace AsyncMapper
{
    public interface IAsyncMemberConfigurationExpression<TSource, TDestination, TMember>
    {
        AsyncResolverConfig _config { get; set; }

        void AddMemberResolver<TMemberResolver, TSourceMember>(Expression<Func<TSource, TSourceMember>> sourceMember) where TMemberResolver : IAsyncMemberValueResolver<TSource, TDestination, TSourceMember, TMember>;
        void AddResolver<TResolver>() where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>;
    }
}