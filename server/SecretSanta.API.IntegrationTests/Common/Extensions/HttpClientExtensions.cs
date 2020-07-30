using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SecretSanta.API.IntegrationTests.Common.Extensions
{
	public static class HttpClientExtensions
	{
		public static async Task<TResponse> PostJson<TResponse>(this HttpClient client, string url, object body)
		{
			var content = new StringContent(JsonConvert.SerializeObject(body));

			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var response = await client.PostAsync(url, content);

			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TResponse>(responseContent);
		}

		public static async Task<TResponse> GetJsonAsync<TResponse>(this HttpClient client, string url)
		{
			var response = await client.GetAsync(url);

			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TResponse>(responseContent);
		}
	}
}
