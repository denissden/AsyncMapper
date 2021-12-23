using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public interface IAsyncMapper : ISyncConfig<IMapper>
    {
        /// <summary>
        /// Execute mapping from source object to a new destination object.
        /// </summary>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Mapped destination object</returns>
        Task<TDestination> Map<TDestination>(object source);
        /// <summary>
        /// Execute mapping from source object to a new destination object.
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Mapped destination object</returns>
        Task<TDestination> Map<TSource, TDestination>(TSource source);

        /// <summary>
        /// Execute mapping from source object to a new destination object.
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object instance</param>
        /// <returns>The mapped destination object, same as the <paramref name="destination"/> object </returns>
        Task<TDestination> Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}