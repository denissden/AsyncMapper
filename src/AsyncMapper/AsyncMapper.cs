using AutoMapper;
using System;

namespace AsyncMapper
{

    public class AsyncMapper
    {   
        public Mapper mapper;
        public AsyncMapper(IConfigurationProvider configurationProvider)
        {   
            mapper = new Mapper(configurationProvider);
            var asyncConf = (AsyncMapperConfiguration)configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            Console.WriteLine($"Created async mapper with maps: \n{String.Join( "\n", asyncConf.conf)}");
        }

        public TDestination Map<TDestination>(object source) => Map(source, default(TDestination));
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}