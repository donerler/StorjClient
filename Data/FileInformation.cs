using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StorjClient.Data
{
    public class FileInformation
    {
        public DateTime DateTime { get; set; }
        public string FileHash { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public IList<Upload> Uploads { get; set; }
        public string Version { get; set; }
    }
    public class Upload
    {
        [JsonProperty("host_name")]
        public string HostName { get; set; }
        public string Url { get; set; }
    }
}
