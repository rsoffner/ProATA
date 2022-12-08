using Newtonsoft.Json;

namespace TaskProcessing.Data.Entities
{
    public class SchedulerDto
    {
        [JsonProperty("id")]
        public virtual Guid Id { get; set; }

        [JsonProperty("hostName")]
        public virtual string HostName { get; set; }

        [JsonProperty("defaultHost")]
        public virtual bool DefaultHost { get; set; }
    }
}
