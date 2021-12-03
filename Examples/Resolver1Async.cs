using AutoMapper;

namespace MapperExperiments;

public class Resolver1Async : IValueResolver<From1, To1, string>
{
    public async Task<string> Resolve(From1 source, To1 dest, string member, ResolutionContext context)
    {
        Console.WriteLine($"Call {nameof(Resolver1)}");
        try {
            HttpResponseMessage res = await Http.httpClient.GetAsync(
                Http.address + "upper/" + source.StringValue
            );
            res.EnsureSuccessStatusCode();
            return res.Content.ReadAsStringAsync().Result;
        }
        catch(HttpRequestException e){
            return "ERROR!";
        }
    }
}