using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMapper.Examples
{
    public class LoginToAddressResolver : IAsyncMemberValueResolver<object, object, string, string>
    {
        IHttpClientFactory _clientFactory;
        public LoginToAddressResolver(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<string> Resolve(object source, object destination, string sourceMember)
        {
            Console.WriteLine($"Call {nameof(LoginToAddressResolver)} {DateTime.Now}");

            using var client = _clientFactory.CreateClient("Api");
            string address = await client.GetStringAsync($"/person?login={sourceMember}&prop=address");
            return address;
        }
    }
}
