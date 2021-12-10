using AutoMapper;
using System.Threading.Tasks;

namespace AsyncMapper
{
    public interface IAsyncMapper
    {
        IMapper _mapper { get; set; }

        Task<TDestination> Map<TDestination>(object source);
        Task<TDestination> Map<TSource, TDestination>(TSource source);
        Task<TDestination> Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}