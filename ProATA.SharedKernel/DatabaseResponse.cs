using Newtonsoft.Json;

namespace ProATA.SharedKernel
{
    public class DatabaseResponse<T>
    {
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty("data")]
        public IEnumerable<T>? Data { get; set; }
    }
}
