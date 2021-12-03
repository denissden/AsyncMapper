using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Linq.Expressions;
using System;

namespace AsyncMapper
{
    public class AsyncMappingExpression<TSource, TDestination> :
        MappingExpression<TSource, TDestination>, IAsyncMappingExpression<TSource, TDestination>
    {
        
        public AsyncMappingExpression(MemberList memberList) : base(memberList) { }
        //public IMappingAction<TSource, TDestination> ResolveAsync()

        public IAsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TMember, TResolver>(
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<IMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>
        {
            var memberInfo = ReflectionHelper.FindProperty(destinationMember);
            return this;
        }

        public IAsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TMember, TResolver>(
            Expression<Func<TDestination, TMember>> destinationMember)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>
        {
            var memberInfo = ReflectionHelper.FindProperty(destinationMember);
            return this;
        }

    }
}