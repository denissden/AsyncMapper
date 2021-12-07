using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Collections.Generic;
using System;

namespace AsyncMapper
{

    public class AsyncMapperConfigurationExpression : MapperConfigurationExpression, IAsyncMapperConfigurationExpression
    {
        public Dictionary<TypePair, IAsyncMappingExpression> _configuredAsyncMaps { get; set; }
        public List<AsyncProfile> _asyncProfiles = new();
        public AsyncMapperConfigurationExpression() : base()
        {
            _configuredAsyncMaps = new();
        }

        public AsyncMappingExpression<TSource, TDestination> CreateAsyncMap<TSource, TDestination>()
        {   
            Console.WriteLine($"Create new async map: {typeof(TSource).Name} to {typeof(TDestination).Name}");
            AsyncMappingExpression<TSource, TDestination> expr = new(
                CreateMap<TSource, TDestination>()
            );
            _configuredAsyncMaps.Add(new TypePair(typeof(TSource), typeof(TDestination)), expr);
            return expr;
        } 

        public void AddAsyncProfile(AsyncProfile asyncProfile)
        {
            _asyncProfiles.Add(asyncProfile);
            AddProfile(asyncProfile);
        }

        public void AddAsyncProfile<TProfile>()
            where TProfile : AsyncProfile, new() => AddAsyncProfile(new TProfile());
    }
}