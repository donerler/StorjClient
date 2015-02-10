using StorjClient.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StorjClient
{
    public class StorjApiClient : StorjClientBase
    {
        private readonly string apiUrl;
        public StorjApiClient(string apiUrl)
            : base(apiUrl)
        {
            this.apiUrl = apiUrl;
        }

        public async Task<UploadedFile> UploadAsync(Func<Stream, HttpContent, TransportContext, Task> onStreamAvailable, string fileName)
        {
            HttpClient client = GetHttpClient();

            MultipartFormDataContent data = new MultipartFormDataContent();
            //data.Add(new StringContent(token), "token");
            data.Add(new PushStreamContent(onStreamAvailable), "file", fileName);

            HttpResponseMessage response = await client.PostAsync("api/upload", data);
            UploadedFile result = await GetContent<UploadedFile>(response.Content);

            return DisposeAndReturnResults(result, data, client);
        }

        public async Task<UploadedFile> UploadStreamedAsync(Func<Stream, Task> onStreamAvailable, string fileName, long length)
        {
            string formDataBoundary = String.Format("{0}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=\"" + formDataBoundary + "\"";
            byte[] header = Encoding.UTF8.GetBytes(string.Format("--{0}\r\nContent-Type: application/octet-stream\r\nContent-Disposition: form-data; name=file; filename=\"{1}\"; filename*=utf-8''{2}\r\n\r\n", formDataBoundary, fileName, Uri.EscapeUriString(fileName)));
            byte[] footer = Encoding.UTF8.GetBytes(string.Format("\r\n--{0}--", formDataBoundary));

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(apiUrl + "api/upload");
            httpWebRequest.Method = "POST";
            httpWebRequest.AllowWriteStreamBuffering = false;
            httpWebRequest.ContentType = contentType;
            httpWebRequest.ContentLength = length + header.Length + footer.Length;
            httpWebRequest.Timeout = int.MaxValue;

            Stream stream = httpWebRequest.GetRequestStream();

            stream.Write(header, 0, header.Length); 
            
            await onStreamAvailable(stream);

            stream.Write(footer, 0, footer.Length);

            stream.Close();

            WebResponse response = await httpWebRequest.GetResponseAsync();

            return GetContent<UploadedFile>(response.GetResponseStream());
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
            HttpWebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);

            HttpClient client = GetHttpClient();

            HttpResponseMessage response = await client.GetAsync("api/download/" + hash + "?key=" + key);

            byte[] result = await response.Content.ReadAsByteArrayAsync();

            return DisposeAndReturnResults(result, client);
        }

        public async Task<Stream> DownloadStreamedAsync(string hash, string key)
        {
            HttpWebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(apiUrl + "api/download/" + hash + "?key=" + key);
            httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
            httpWebRequest.Method = "GET";
            httpWebRequest.AllowReadStreamBuffering = false;
            httpWebRequest.Timeout = int.MaxValue;

            WebResponse response = await httpWebRequest.GetResponseAsync();

            return response.GetResponseStream();
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
