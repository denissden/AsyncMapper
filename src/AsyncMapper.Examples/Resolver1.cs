using AutoMapper;
using System.Net.Http;
using System;

namespace AsyncMapper.Examples
{
    public class Resolver1 : IValueResolver<From1, To1, string>
    {
        public string Resolve(From1 source, To1 dest, string member, ResolutionContext context)
        {
            Console.WriteLine($"Call {nameof(Resolver1)}");
            try
            {
                HttpResponseMessage res = Http.httpClient.GetAsync(
                    Http.address + "upper/" + source.StringValue
                ).GetAwaiter().GetResult();
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