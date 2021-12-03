using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;

namespace AsyncMapper;

public class AsyncMapperExpression<TSource, TDestination> : MappingExpression<TSource, TDestination>
{
    public AsyncMapperExpression(MemberList memberList) : base(memberList) {}
    public IMappingAction<TSource, TDestination> ResolveAsync()
}