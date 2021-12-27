using AutoMapper;
using System.ComponentModel;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using AsyncMapper;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace AsyncMapper.Examples {
    public partial class Program
    {
        public static async Task Main()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<HttpClient>();
            services.AddTransient<ISomeService>(sp => new SomeService((i) => i * 2));
            services.AddAsyncMapper(typeof(MarkerType));

            var provider = services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var mapper = scope.ServiceProvider.GetService<IAsyncMapper>();

                var f1 = new From1(12, "fddfwf");

                Console.WriteLine(f1);

                var t1 = await mapper.Map<To1>(f1);

                Console.WriteLine(t1);

                var f2 = new From2(10, 1, "asdf");

                Console.WriteLine(f2);

                var t2 = await mapper.Map<To2>(f2);

                Console.WriteLine(t2);
            }
            
        }
    }
}