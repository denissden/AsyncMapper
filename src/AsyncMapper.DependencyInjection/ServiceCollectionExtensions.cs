using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AsyncMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAsyncMapper(this IServiceCollection services, params Type[] markerTypes) =>
            AddAsyncMapperCore(services, markerTypes.Select(t => t.GetTypeInfo().Assembly));

        public static IServiceCollection AddAsyncMapperCore(IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
        {
            services.Configure<AsyncMapperConfigurationExpression>(o => o.AddAsyncProfiles(assembliesToScan.ToArray()));

            // check duplicate service
            if (services.Any(sd => sd.ServiceType == typeof(IAsyncMapper)))
                return services;

            services.AddSingleton<AsyncMapperConfiguration>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<AsyncMapperConfigurationExpression>>();
                return new AsyncMapperConfiguration(options.Value);
            });

            // add sync mapper via DI so it supports DI
            services.AddAutoMapper(assembliesToScan);

            services.Add(new ServiceDescriptor(typeof(IAsyncMapper),
                serviceProvider => new AsyncMapper.Mapper(
                    serviceProvider.GetRequiredService<AsyncMapperConfiguration>(),
                    serviceProvider.GetService),
                ServiceLifetime.Transient));

            return services;
        }
    }
}
