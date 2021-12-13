using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

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
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));
            //if (_configuredAsyncMaps.ContainsKey(typePair))
            //    throw new ArgumentException($"Map from {typeof(TSource).Name} to {typeof(TDestination).Name} has already been configured.");
            _configuredAsyncMaps.Add(typePair, expr);
            return expr;
        } 

        public void AddAsyncProfile(AsyncProfile asyncProfile)
        {
            _asyncProfiles.Add(asyncProfile);
            AddProfile(asyncProfile);
        }

        public void AddAsyncProfile<TProfile>()
            where TProfile : AsyncProfile, new() => AddAsyncProfile(new TProfile());

        public void AddAsyncProfiles(params Assembly[] assembliesToScan) =>
            AddAsyncProfilesCore(assembliesToScan);
        

        public void AddAsyncProfiles(params Type[] typesToScan) => 
            AddAsyncProfilesCore(typesToScan.Select(t => t.Assembly));

        void AddAsyncProfilesCore(IEnumerable<Assembly> assembliesToScan)
        {
            foreach (var a in assembliesToScan)
            {
                var profileTypes = a.GetTypes().Where(t => t.IsSubclassOf(typeof(AsyncProfile)));
                foreach (var profileType in profileTypes)
                {
                    AddAsyncProfile((AsyncProfile)Activator.CreateInstance(profileType));
                }
            }
        }

    }
}