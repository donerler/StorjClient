using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StorjClient
{
    public abstract class StorjClientBase
    {
        private readonly string apiUrl;
        private static readonly Lazy<JsonSerializerSettings> jsonSettings = new Lazy<JsonSerializerSettings>(() => CreateSerializerSettings());

        protected StorjClientBase(string apiUrl)
        {
            this.apiUrl = apiUrl;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            HttpClient client = GetHttpClient();

            HttpResponseMessage response = await client.GetAsync(url);

            T result = await GetContent<T>(response.Content);

            return DisposeAndReturnResults(result, client);
        }

        protected async Task<T> PostAsync<T>(string url, string secret = null)
        {
            HttpClient client = GetHttpClient();

            HttpContent content = new StringContent(string.Empty);

            if (!string.IsNullOrEmpty(secret))
            {
                content.Headers.Add("Authentication", secret);
            }

            HttpResponseMessage response = await client.PostAsync(url, content);

            T result = await GetContent<T>(response.Content);

            return DisposeAndReturnResults(result, content, client);
        }

        private static JsonSerializerSettings CreateSerializerSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();

            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }

        protected async Task<T> GetContent<T>(HttpContent content)
        {
            string result = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result, jsonSettings.Value);
        }

        protected T GetContent<T>(Stream stream)
        {
            JsonSerializer serializer = new JsonSerializer();
            
            return serializer.Deserialize<T>(new JsonTextReader(new StreamReader(stream)));
        }

        protected T DisposeAndReturnResults<T>(T results, params IDisposable[] disposableObjects)
        {
            foreach (IDisposable disposable in disposableObjects)
            {
                try
                {
                    disposable.Dispose();
                }
                catch { }
            }

            return results;
        }

        protected HttpClient GetHttpClient()
        {
            return new HttpClient { BaseAddress = new Uri(apiUrl) };
        }
    }
}
