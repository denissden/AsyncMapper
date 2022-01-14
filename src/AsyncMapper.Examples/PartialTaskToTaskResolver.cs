using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AsyncMapper.Examples
{
    public class PartialTaskToTaskResolver : IAsyncValueResolver<PartialTask, Task, string>
    {
        IHttpClientFactory _clientFactory;
        public PartialTaskToTaskResolver(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> Resolve(PartialTask source, Task destination)
        {
            Console.WriteLine($"Call {nameof(PartialTaskToTaskResolver)}");

            using var client = _clientFactory.CreateClient("Api");
            destination.responsible_full_name = await client.GetStringAsync($"/person?login={source.responsible_login}&prop=name");
            string taskString = await client.GetStringAsync($"/task?id={source.id}&prop=description");
            var task = JsonSerializer.Deserialize<Task>(taskString);
            return task.description;
        }
    }
}
