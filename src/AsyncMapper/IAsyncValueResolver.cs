using System.Threading.Tasks;

namespace AsyncMapper
{
    public interface IAsyncValueResolver<in TSource, in TDestination, TDestMember>
    {
        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="destMember">Destination member</param>
        /// <returns>Result, typically build from the source resolution result</returns>
        Task<TDestMember> Resolve(TSource source, TDestination destination, TDestMember destMember);
    }
}