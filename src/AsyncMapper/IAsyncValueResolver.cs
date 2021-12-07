using System.Threading.Tasks;

namespace AsyncMapper
{
    public interface IAsyncValueResolver<in TSource, in TDestination, TDestMember> : IAsyncValueResolver
    {
        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <returns>Result, typically build from the source resolution result</returns>
        Task<TDestMember> Resolve(TSource source, TDestination destination);
    }

    public interface IAsyncValueResolver { }


    public interface IAsyncMemberValueResolver<in TSource, in TDestination, in TSourceMember, TDestMember> : IAsyncMemberValueResolver
    {
        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="sourceMember">Source member</param>
        /// <returns>Result, typically build from the source resolution result</returns>
        Task<TDestMember> Resolve(TSource source, TDestination destination, TSourceMember sourceMember);
    }

    public interface IAsyncMemberValueResolver { }
}