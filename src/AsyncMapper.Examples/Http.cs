using System.Net.Http;

namespace AsyncMapper.Examples
{
    public static class Http
    {
        public static readonly HttpClient httpClient = new();
        public static readonly string address = @"http://127.0.0.1:5000/";
    }
}