using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Examples
{
    public class LoginToJobResolver : IAsyncMemberValueResolver<object, object, string, Job>
    {
        IHttpClientFactory _clientFactory;
        public LoginToJobResolver(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<Job> Resolve(object source, object destination, string sourceMember)
        {
            Console.WriteLine($"Call {nameof(LoginToJobResolver)} {DateTime.Now}");

            using var client = _clientFactory.CreateClient("Api");

            var jobName = client.GetStringAsync($"/person?login={sourceMember}&prop=job");
            var employmentDateString = client.GetStringAsync($"/person?login={sourceMember}&prop=employment_date");
            var employmentDate = DateTime.Parse(await employmentDateString, null, System.Globalization.DateTimeStyles.RoundtripKind);
            return new Job { name = await jobName, employment_date = employmentDate };
        }
    }
}
