using Newtonsoft.Json;

namespace StorjClient.Data
{
    public class Status
    {
        public Bandwidth BandWidth { get; set; }
        public Storage Storage { get; set; }
        public Sync Sync { get; set; }
    }

    public class Current
    {
        public long Incoming { get; set; }
        public long Outgoing { get; set; }
    }

    public class Limits
    {
        public long Incoming { get; set; }
        public long Outgoing { get; set; }
    }

    public class Total
    {
        public long Incoming { get; set; }
        public long Outgoing { get; set; }
    }

    public class Bandwidth
    {
        public Current Current { get; set; }
        public Limits Limits { get; set; }
        public Total Total { get; set; }
    }

    public class Storage
    {
        public long Capacity { get; set; }
        [JsonProperty("max_file_size")]
        public int MaxFileSize { get; set; }
        public long Used { get; set; }
    }

    public class BlockchainQueue
    {
        public int Count { get; set; }
        public int Size { get; set; }
    }

    public class CloudQueue
    {
        public int Count { get; set; }
        public long Size { get; set; }
    }

    public class Sync
    {
        [JsonProperty("blockchain_queue")]
        public BlockchainQueue BlockchainQueue { get; set; }
        [JsonProperty("cloud_queue")]
        public CloudQueue CloudQueue { get; set; }
    }
}
