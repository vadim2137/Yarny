using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Yarny.Api
{
    public static class ApiClient
    {
        private const string BaseUrl = "http://178.236.252.111:5000/";
        private static readonly HttpClientHandler handler = new HttpClientHandler();
        private static readonly HttpClient client = new HttpClient(handler);

        static ApiClient()
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        }

        public static async Task<string?> SendRequestAsync(
            string endpoint,
            object requestData,
            HttpMethod method,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var fullUrl = BaseUrl + endpoint;
                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(method, fullUrl)
                {
                    Content = content
                };

                request.Headers.Add("Accept", "application/json");

                var response = await client.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadAsStringAsync();
            }
            catch (TaskCanceledException)
            {
                return "Request timeout";
            }
            catch (HttpRequestException ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}