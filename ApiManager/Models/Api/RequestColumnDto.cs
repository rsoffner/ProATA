using Newtonsoft.Json;

namespace ApiManager.Models.Api
{
    public class RequestColumnDto
    {
        [JsonProperty("data")]
        public int Data { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("searchable")]
        public bool Searchable { get; set; }

        [JsonProperty("orderable")]
        public bool Orderable { get; set; }

        [JsonProperty("search")]
        public RequestSearchDto Search { get; set; }
    }
}
