using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AsyncMapper
{

    public class AsyncMapperConfigurationExpression : MapperConfigurationExpression, MAsyncMapperConfigurationExpression
    {

    }

    public static class AsyncMapperConfigurationExpressionExtensions
    {
        public static void AddAsyncProfile(this AsyncMapperConfigurationExpression instance, AsyncProfile asyncProfile)
        {
            instance.GetFields()._asyncProfiles.Add(asyncProfile);
            instance.AddProfile(asyncProfile);
        }

        public static void AddAsyncProfile<TProfile>(this AsyncMapperConfigurationExpression instance)
            where TProfile : AsyncProfile, new() => instance.AddAsyncProfile(new TProfile());

        public static void AddAsyncProfiles(this AsyncMapperConfigurationExpression instance, params Assembly[] assembliesToScan) =>
            instance.AddAsyncProfilesCore(assembliesToScan);


        public static void AddAsyncProfiles(this AsyncMapperConfigurationExpression instance, params Type[] typesToScan) =>
            instance.AddAsyncProfilesCore(typesToScan.Select(t => t.Assembly));

        static void AddAsyncProfilesCore(this AsyncMapperConfigurationExpression instance, IEnumerable<Assembly> assembliesToScan)
        {
            foreach (var a in assembliesToScan)
            {
                var profileTypes = a.GetTypes().Where(t => t.IsSubclassOf(typeof(AsyncProfile)));
                foreach (var profileType in profileTypes)
                {
                    instance.AddAsyncProfile((AsyncProfile)Activator.CreateInstance(profileType));
                }
            }
        }
    }

    // marker interface
    // MapperConfigurationExpression is inherited from Profile in AutoMapper, so inherit here too
    public interface MAsyncMapperConfigurationExpression : MAsyncProfile { }
}