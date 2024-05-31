using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ValconLibrary.IntegrationTests.Helpers
{
    public class GetRequestContent
    {
        public async Task<T?> GetRequestContentAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var jsonSettings = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            return JsonSerializer.Deserialize<T>(
                await httpResponseMessage.Content.ReadAsStringAsync(),
                jsonSettings);
        }
    }
}
