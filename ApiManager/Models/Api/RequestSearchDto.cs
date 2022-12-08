using Newtonsoft.Json;

namespace ApiManager.Models.Api
{
    public class RequestSearchDto
    {
        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("regex")]
        public bool Regex { get; set; }
    }
}