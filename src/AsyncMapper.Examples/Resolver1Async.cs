using AutoMapper;
using AsyncMapper;
using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace AsyncMapper.Examples
{
    public class Resolver1Async : IAsyncValueResolver<From1, To1, string>
    {
        private readonly HttpClient _httpClient;
        public Resolver1Async(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> Resolve(From1 source, To1 dest)
        {
            Console.WriteLine($"Call {nameof(Resolver1)}");
            try
            {
                HttpResponseMessage res = await _httpClient.GetAsync(
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