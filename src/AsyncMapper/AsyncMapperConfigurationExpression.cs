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
    /// <summary>
    /// Marks <see cref="MapperConfigurationExpression"/>. This was extension methods only show when configuring <see cref="AsyncMapper"/>
    /// </summary>
    public class AsyncMapperConfigurationExpression : MapperConfigurationExpression, MAsyncMapperConfigurationExpression
    {
        
    }

    /// <summary>
    /// Extensions for <see cref="MapperConfigurationExpression"/>
    /// </summary>
    public static class AsyncMapperConfigurationExpressionExtensions
    {
        /// <summary>
        /// Add an existing profile
        /// </summary>
        /// <param name="asyncProfile">Profile instance</param>
        public static void AddAsyncProfile(this AsyncMapperConfigurationExpression instance, AsyncProfile asyncProfile)
        {
            instance.GetFields()._asyncProfiles.Add(asyncProfile);
            instance.AddProfile(asyncProfile);
        }

        /// <summary>
        /// Add an existing profile
        /// </summary>
        /// <typeparam name="TProfile">Profile type</typeparam>
        public static void AddAsyncProfile<TProfile>(this AsyncMapperConfigurationExpression instance)
            where TProfile : AsyncProfile, new() => instance.AddAsyncProfile(new TProfile());

        /// <summary>
        /// Add mapping definitions contained in assemblies.
        /// Looks for <see cref="AsyncProfile"/> definitions
        /// </summary>
        /// <param name="assembliesToScan">Assemblies containing profiles</param>
        public static void AddAsyncProfiles(this AsyncMapperConfigurationExpression instance, params Assembly[] assembliesToScan) =>
            instance.AddAsyncProfilesCore(assembliesToScan);

        /// <summary>
        /// Add mapping definitions contained in assemblies.
        /// Looks for <see cref="AsyncProfile"/> definitions
        /// </summary>
        /// <param name="typesToScan">Types from assemblies containing profiles</param>
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

    /// <summary>
    /// Marker
    /// </summary>
    public interface MAsyncMapperConfigurationExpression : MAsyncProfile { }
}