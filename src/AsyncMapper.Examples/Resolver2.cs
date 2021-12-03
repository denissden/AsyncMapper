using AutoMapper;
using System;
using System.Net.Http;

namespace AsyncMapper.Examples
{
    public class Resolver2 : IValueResolver<From2, To2, int>
    {
        public int Resolve(From2 source, To2 dest, int member, ResolutionContext context)
        {
            Console.WriteLine($"Call {nameof(Resolver2)}");
            try
            {
                HttpResponseMessage res = Http.httpClient.GetAsync(
                    Http.address + "plus/" + source.IntValue2
                ).GetAwaiter().GetResult();
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