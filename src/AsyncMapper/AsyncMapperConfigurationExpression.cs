using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace AsyncMapper
{

    public class AsyncMapperConfigurationExpression : 
        AsyncProfile, 
        IAsyncMapperConfigurationExpression
    {
        public List<AsyncProfile> _asyncProfiles = new();
        internal MapperConfigurationExpression _mapperConfigurationExpression;
        public override MapperConfigurationExpression Sync => _mapperConfigurationExpression;

        public AsyncMapperConfigurationExpression() : base()
        {
            _mapperConfigurationExpression = new();
        }

        /// <summary>
        /// Since the class is not inherited from <see cref="MapperConfigurationExpression"/>,
        /// create a map using a function from it's instance
        /// </summary>
        /// <returns>Mapping expression (syncronous)</returns>
        internal override IMappingExpression<TSource, TDestination> CreateMappingExpression<TSource, TDestination>()
        {
            return _mapperConfigurationExpression.CreateMap<TSource, TDestination>();
        }

        public void AddAsyncProfile(AsyncProfile asyncProfile)
        {
            _asyncProfiles.Add(asyncProfile);
            _mapperConfigurationExpression.AddProfile(asyncProfile.Sync);
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