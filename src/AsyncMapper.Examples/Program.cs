using AutoMapper;
using System.ComponentModel;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using AsyncMapper;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;

namespace AsyncMapper.Examples {
    public partial class Program
    {
        static async System.Threading.Tasks.Task Main()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<HttpClient>();
            services.AddTransient<ISomeService>(sp => new SomeService((i) => i * 2));
            services.AddAsyncMapper(typeof(ProfileMarkerType));
            services.AddHttpClient("Api", httpClient => {
                httpClient.BaseAddress = new Uri("http://127.0.0.1:5000");
            });
            services.AddHttpClient("FastApi", httpClient => {
                httpClient.BaseAddress = new Uri("http://127.0.0.1:5000");
                httpClient.DefaultRequestHeaders.Add("no-delay", "");
            });

            var provider = services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                // fetch all data from server with no added delay
                var httpFactory = scope.ServiceProvider.GetService<IHttpClientFactory>();
                var fastClient = httpFactory.CreateClient("FastApi");
                var people = JsonSerializer.Deserialize<List<Person>>(await fastClient.GetStringAsync("/person?id=all"));
                var tasks = JsonSerializer.Deserialize<List<PartialTask>>(await fastClient.GetStringAsync("/task?id=all"));
                var mapper = scope.ServiceProvider.GetService<IAsyncMapper>();

                // prints when each mapping starts and ends
                int c;
                async Task<TDestination> MapWithLogging<TSource, TDestination>(TSource p)
                {
                    int index = c++;
                    Console.WriteLine($"Start mapping {typeof(TSource).Name} {index} into {typeof(TDestination).Name} at {DateTime.Now}");
                    var ret = await mapper.Map<TDestination>(p);
                    Console.WriteLine($"End mapping {typeof(TSource).Name} {index} into {typeof(TDestination).Name} at {DateTime.Now}");
                    return ret;
                }
                // map Person to Worker
                // runs all maps in parallel
                // each map calls 2 resolvers
                c = 0;
                Console.WriteLine($"Start mapping people at {DateTime.Now}");
                var workers = await mapper.Map<Worker>(people);
                Console.WriteLine($"End mapping people at {DateTime.Now}");

                // map Worker back to Person
                // runs instantly because no resolvers are needed
                Console.WriteLine($"Start mapping workers at {DateTime.Now}");
                Person[] _ = await System.Threading.Tasks.Task.WhenAll(people.Select(p => mapper.Map<Person>(p)));
                Console.WriteLine($"End mapping workers at {DateTime.Now}");

                // map PartialTask to Task
                // runs all maps in parallel
                // each map calls 2 resolvers
                c = 0;
                Console.WriteLine($"Start mapping tasks at {DateTime.Now}");
                Task[] fullTasks = await System.Threading.Tasks.Task.WhenAll(tasks.Select(t => MapWithLogging<PartialTask, Task>(t)));
                Console.WriteLine($"End mapping tasks at {DateTime.Now}");

                // map Task back to PartialTask
                // runs instantly because no resolvers are needed
                Console.WriteLine($"Start mapping tasks at {DateTime.Now}");
                await System.Threading.Tasks.Task.WhenAll(fullTasks.Select(t => mapper.Map<PartialTask>(t)));
                Console.WriteLine($"End mapping tasks at {DateTime.Now}");
            }
        }
    }
}