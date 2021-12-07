using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public class AsyncMappingExpression<TSource, TDestination> : MappingExpression<TSource, TDestination>, IMappingExpression<TSource, TDestination>, IAsyncMappingExpression
    {   
        public IMappingExpression<TSource, TDestination> mappingExpression;

        public List<AsyncResolverConfig> conf { get; set; }

        // public AsyncMappingExpression(MemberList memberList) : base(memberList)
        // {
        // }

        // public AsyncMappingExpression() : this(new MemberList())
        // {
        // }

        public AsyncMappingExpression(IMappingExpression<TSource, TDestination> fromMap) : base(new MemberList())
        {
            conf = new();
            mappingExpression = fromMap;
        }

        public AsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TMember, TResolver>(
            Expression<Func<TDestination, TMember>> destinationMember)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>
        {
            var memberInfo = ReflectionHelper.FindProperty(destinationMember);
            var resolverConfig = new AsyncResolverConfig ()
            {
                MemberGetter = destinationMember.Compile(),
                ToMemberInfo = memberInfo,
                ResolverType = typeof(TResolver),
                MemberType = typeof(TMember),
                MemberTaskType = typeof(Task<TMember>),
            };
            var a = typeof(Task);
            conf.Add(resolverConfig);
            // the property will be mapped by async mapper so ignore it
            mappingExpression.ForMember(destinationMember, opt => opt.Ignore());
            Console.WriteLine($"AsyncMappingExpression: new member {memberInfo.Name} with resolver {typeof(TResolver).Name}");
            return this;
        }

        public IMappingExpression<TSource, TDestination> EndAsyncConfig() => mappingExpression;
    }

    public interface IAsyncMappingExpression {
        public List<AsyncResolverConfig> conf { get; set; }
    }
}