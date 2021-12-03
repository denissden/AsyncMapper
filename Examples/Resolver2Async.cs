using AutoMapper;

namespace MapperExperiments;

public class Resolver2Async : IValueResolver<From2, To2, int>
{
    public async Task<int> ResolveAsync(From2 source, To2 dest, int member, ResolutionContext context)
    {
        Console.WriteLine($"Call {nameof(Resolver2)}");
        try {
            HttpResponseMessage res = await Http.httpClient.GetAsync(
                Http.address + "plus/" + source.IntValue2
            );
            res.EnsureSuccessStatusCode();
            var str =  res.Content.ReadAsStringAsync().Result;
            return Convert.ToInt32(str);
        }
        catch(HttpRequestException e){
            return -1;
        }
    }
}