using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace AsyncMapper
{
    public class AsyncMappingExpression<TSource, TDestination> : MappingExpression<TSource, TDestination>, IMappingExpression<TSource, TDestination>
    {   
        public IMappingExpression<TSource, TDestination> mappingExpression;
        
        public List<string> conf = new List<string>();

        public AsyncMappingExpression(MemberList memberList) : base(memberList)
        {
        }

        public AsyncMappingExpression() : base(new MemberList())
        {
        }

        public AsyncMappingExpression(IMappingExpression<TSource, TDestination> fromMap) : base(new MemberList())
        {
            mappingExpression = fromMap;
        }
        //public IMappingAction<TSource, TDestination> ResolveAsync()

        public AsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TMember, TResolver>(
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<IMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>
        {
            var memberInfo = ReflectionHelper.FindProperty(destinationMember);
            conf.Add(memberInfo.Name);
            Console.WriteLine($"AsyncMappingExpression: new member {memberInfo.Name}");
            return this;
        }

        public AsyncMappingExpression<TSource, TDestination> AddAsyncResolver<TMember, TResolver>(
            Expression<Func<TDestination, TMember>> destinationMember)
            where TResolver : IAsyncValueResolver<TSource, TDestination, TMember>
        {
            var memberInfo = ReflectionHelper.FindProperty(destinationMember);
            conf.Add(memberInfo.Name);
            // the property will be mapped by async mapper so ignore it
            //mappingExpression.ForMember(destinationMember, opt => opt.Ignore());
            Console.WriteLine($"AsyncMappingExpression: new member {memberInfo.Name} with resolver {typeof(TResolver).Name}");
            return this;
        }

        public IMappingExpression<TSource, TDestination> EndAsyncConfig() => mappingExpression;

        
    }
}