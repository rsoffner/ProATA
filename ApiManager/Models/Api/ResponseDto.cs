using Newtonsoft.Json;

namespace ApiManager.Models.Api
{
    public class ResponseDto<T>
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty("data")]
        public IList<T> Data { get; set; }
    }
}
