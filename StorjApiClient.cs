using StorjClient.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StorjClient
{
    public class StorjApiClient : StorjClientBase
    {
        public StorjApiClient(string apiUrl)
            : base(apiUrl)
        {
        }

        public async Task<UploadedFile> UploadAsync(Stream content, string fileName)
        {
            HttpClient client = GetHttpClient();

            MultipartFormDataContent data = new MultipartFormDataContent();
            //data.Add(new StringContent(token), "token");
            data.Add(new StreamContent(content), "file", fileName);

            HttpResponseMessage response = await client.PostAsync("api/upload", data);
            UploadedFile result = await GetContent<UploadedFile>(response.Content);

            return DisposeAndReturnResults(result, data, client);
        }

        public async Task<byte[]> DownloadAsync(string hash, string key)
        {
            HttpClient client = GetHttpClient();

            HttpResponseMessage response = await client.GetAsync("api/download/" + hash + "?key=" + key);

            byte[] result = await response.Content.ReadAsByteArrayAsync();

            return DisposeAndReturnResults(result, client);
        }

        public async Task<FileInformation> Find(string hash)
        {
            return await GetAsync<FileInformation>("api/find/" + hash);
        }

        public async Task<Status> Status()
        {
            return await GetAsync<Status>("api/status/");
        }
    }
}
