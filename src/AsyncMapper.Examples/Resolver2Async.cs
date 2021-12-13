using AutoMapper;
using AsyncMapper;
using System.Threading.Tasks;
using System;
using System.Net.Http;

namespace AsyncMapper.Examples
{
    public class Resolver2Async : IAsyncMemberValueResolver<From2, To2, int, int>
    {
        private readonly HttpClient _httpClient;
        private readonly ISomeService _someService;
        public Resolver2Async(HttpClient httpClient, ISomeService someService)
        {
            _httpClient = httpClient;
            _someService = someService;
        }
        public async Task<int> Resolve(From2 source, To2 dest, int sourceMember)
        {
            Console.WriteLine($"Call {nameof(Resolver2)}");
            try
            {
                HttpResponseMessage res = await _httpClient.GetAsync(
                    Http.address + "plus/" + sourceMember
                );
                res.EnsureSuccessStatusCode();
                var str = res.Content.ReadAsStringAsync().Result;
                int intRes = Convert.ToInt32(str);
                return _someService.Modify(intRes);
            }
            catch (HttpRequestException e)
            {
                return -1;
            }
        }
    }
}