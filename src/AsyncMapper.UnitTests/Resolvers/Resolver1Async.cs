using AutoMapper;
using AsyncMapper;
using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace AsyncMapper.UnitTests
{
    public class Resolver1Async : IAsyncValueResolver<From1, To1, string>
    {
        public async Task<string> Resolve(From1 source, To1 dest)
        {
            Console.WriteLine($"Call {nameof(Resolver1)}");
            try
            {
                HttpResponseMessage res = await Http.httpClient.GetAsync(
                    Http.address + "upper/" + source.StringValue
                );
                res.EnsureSuccessStatusCode();
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException e)
            {
                return "ERROR!";
            }
        }
    }
}