using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;

namespace Funda.Tasks.Api
{
    public static class HttpRequestExtensions
    {
        //string name = req.Query["userId"];
        public async static Task<T> GetBodyAsync<T>(this HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(requestBody);
        }
    }
}
