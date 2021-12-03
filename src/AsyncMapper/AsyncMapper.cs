using AutoMapper;

namespace AsyncMapper
{

    public class AsyncMapper : Mapper
    {
        public AsyncMapper(IConfigurationProvider configurationProvider) : base(configurationProvider) { }

        public TDestination MapAsync<TDestination>(object source) => MapAsync(source, default(TDestination));
        public TDestination MapAsync<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Map<TSource, TDestination>(source, destination);
        }
    }
}