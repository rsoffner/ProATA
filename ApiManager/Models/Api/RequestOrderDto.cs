using Newtonsoft.Json;

namespace ApiManager.Models.Api
{
    public class RequestOrderDto
    {
        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; }
    }
}