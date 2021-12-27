using System;
using System.Reflection;

namespace AsyncMapper
{
    public interface IAsyncMapperConfigurationExpression
    {
        /// <summary>
        /// Add an existing profile
        /// </summary>
        /// <param name="asyncProfile">Profile instance</param>
        void AddAsyncProfile(AsyncProfile asyncProfile);
        /// <summary>
        /// Add an existing profile
        /// </summary>
        /// <typeparam name="TProfile">Profile type</typeparam>
        void AddAsyncProfile<TProfile>() where TProfile : AsyncProfile, new();
        /// <summary>
        /// Add mapping definitions contained in assemblies.
        /// Looks for <see cref="AsyncProfile"/> definitions
        /// </summary>
        /// <param name="assembliesToScan">Assemblies containing profiles</param>
        void AddAsyncProfiles(params Assembly[] assembliesToScan);
        /// <summary>
        /// Add mapping definitions contained in assemblies.
        /// Looks for <see cref="AsyncProfile"/> definitions
        /// </summary>
        /// <param name="typesToScan">Types from assemblies containing profiles</param>
        void AddAsyncProfiles(params Type[] typesToScan);
    }
}