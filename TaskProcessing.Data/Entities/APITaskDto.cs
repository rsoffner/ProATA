using Newtonsoft.Json;

namespace TaskProcessing.Data.Entities
{
    public class APITaskDto
    {
        [JsonProperty("id")]
        public virtual Guid Id { get; set; }

        [JsonProperty("title")]
        public virtual string Title { get; set; }

        [JsonProperty("enabled")]
        public virtual bool Enabled { get; set; }
    }
}
