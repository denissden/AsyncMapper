using AutoMapper;
using AsyncMapper;
using System.Threading.Tasks;
using System;
using System.Net.Http;

namespace AsyncMapper.UnitTests
{
    public class Resolver2Async : IAsyncValueResolver<From2, To2, int>
    {
        public async Task<int> Resolve(From2 source, To2 dest)
        {
            Console.WriteLine($"Call {nameof(Resolver2Async)}");
            try
            {
                HttpResponseMessage res = await Http.httpClient.GetAsync(
                    Http.address + "plus/" + source.IntValue2
                );
                res.EnsureSuccessStatusCode();
                var str = res.Content.ReadAsStringAsync().Result;
                return Convert.ToInt32(str);
            }
            catch (HttpRequestException e)
            {
                return -1;
            }
        }
    }
}