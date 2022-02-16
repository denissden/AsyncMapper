using AutoMapper;
using System;
using System.Collections.Generic;
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
        Task<TDestination> Map<TDestination>(object source, MappingOptions options = MappingOptions.MapInParallel);
        /// <summary>
        /// Execute mapping from source object to a new destination object.
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Mapped destination object</returns>
        Task<TDestination> Map<TSource, TDestination>(TSource source, MappingOptions options = MappingOptions.MapInParallel);

        /// <summary>
        /// Execute mapping from source object to a new destination object.
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object instance</param>
        /// <returns>The mapped destination object, same as the <paramref name="destination"/> object </returns>
        Task<TDestination> Map<TSource, TDestination>(TSource source, TDestination destination, MappingOptions options = MappingOptions.MapInParallel);

        /// <summary>
        /// Execute mapping from <see cref="IEnumerable{T}"/> of source objects to a new <see cref="IEnumerable{T}"/> of destination objects.
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/> of Source objects</param>
        /// <returns>The mapped <see cref="IEnumerable{T}"/> of destination objects</returns>
        Task<IEnumerable<TDestination>> Map<TSource, TDestination>(IEnumerable<TSource> source, MappingOptions options = MappingOptions.MapInParallel);

        /// <summary>
        /// Execute mapping from <see cref="IEnumerable{T}"/> of source objects to a new <see cref="IEnumerable{T}"/> of destination objects.
        /// </summary>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/> of Source objects</param>
        /// <returns>The mapped <see cref="IEnumerable{T}"/> of destination objects</returns>
        Task<IEnumerable<TDestination>> Map<TDestination>(IEnumerable<object> source, MappingOptions options = MappingOptions.MapInParallel);
    }
}