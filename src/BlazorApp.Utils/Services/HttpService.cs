using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BlazorApp.Utils.Services
{
    public class HttpService(HttpClient httpClient) : IHttpService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<T> Get<T>(string uri)
        {
            HttpRequestMessage request = new(HttpMethod.Get, uri);
            return await SendRequest<T>(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            HttpRequestMessage request = new(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
            };

            return await SendRequest<T>(request);
        }

        public async Task<T> Post<T>(string uri)
        {
            HttpRequestMessage request = new(HttpMethod.Post, uri);
            return await SendRequest<T>(request);
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            HttpRequestMessage request = new(HttpMethod.Put, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
            };
            return await SendRequest<T>(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            HttpRequestMessage request = new(HttpMethod.Delete, uri);
            return await SendRequest<T>(request);
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage request)
        {
            try
            {
                using HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }

                return await response.Content.ReadFromJsonAsync<T>() ?? throw new NullReferenceException();
            }
            catch
            {
                throw;
            }
        }
    }
}
