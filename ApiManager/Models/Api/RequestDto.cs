using Newtonsoft.Json;

namespace ApiManager.Models.Api
{
    public class RequestDto
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("order")]
        public IList<RequestOrderDto> Order { get; set; }

        [JsonProperty("search")]
        public RequestSearchDto Search { get; set; }

        [JsonProperty("columns")]
        public IList<RequestColumnDto>  Columns { get; set; }

        [JsonProperty("period")]
        public int Period { get; set; }
    }
}
