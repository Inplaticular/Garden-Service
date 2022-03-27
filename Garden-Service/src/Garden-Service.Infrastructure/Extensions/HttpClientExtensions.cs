using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Inplanticular.Garden_Service.Infrastructure.Extensions;

public static class HttpClientExtensions {
	public static async Task<TResponse?> SendGetAsync<TResponse>(this HttpClient client, string url) {
		try {
			var response = await client.GetAsync(url);
			return await ParseResponse<TResponse>(response);
		}
		catch (Exception) {
			return default;
		}
	}

	public static async Task<TResponse?> SendPostAsync<TRequest, TResponse>(this HttpClient client, string url,
		TRequest request) {
		try {
			var response = await client.PostAsync(url, new StringContent(
				JsonConvert.SerializeObject(request),
				Encoding.UTF8,
				"application/json"
			));

			return await ParseResponse<TResponse>(response);
		}
		catch (Exception) {
			throw;
		}
	}

	public static async Task<TResponse?> SendPutAsync<TRequest, TResponse>(this HttpClient client, string url,
		TRequest request) {
		try {
			var response = await client.PutAsync(url, new StringContent(
				JsonConvert.SerializeObject(request),
				Encoding.UTF8,
				"application/json"
			));

			return await ParseResponse<TResponse>(response);
		}
		catch (Exception) {
			return default;
		}
	}

	public static async Task<TResponse?> SendDeleteAsync<TRequest, TResponse>(this HttpClient client, string url,
		TRequest request) {
		try {
			var response = await client.SendAsync(new HttpRequestMessage {
				Content = new StringContent(
					JsonConvert.SerializeObject(request),
					Encoding.UTF8,
					"application/json"
				),
				Method = HttpMethod.Delete,
				RequestUri = new Uri(url)
			});

			return await ParseResponse<TResponse>(response);
		}
		catch (Exception) {
			return default;
		}
	}

	private static async Task<TResponse?> ParseResponse<TResponse>(HttpResponseMessage response) {
		if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.BadRequest)
			return default;

		var content = await response.Content.ReadAsStringAsync();
		var responseBody = JsonConvert.DeserializeObject<TResponse>(content);
		return responseBody;
	}
}