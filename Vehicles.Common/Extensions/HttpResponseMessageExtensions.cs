using Newtonsoft.Json;

namespace Vehicles.Common.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> GetResponseBodyAsync<T>(this HttpResponseMessage response)
        {
            var contentAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contentAsString);
        }
    }
}
