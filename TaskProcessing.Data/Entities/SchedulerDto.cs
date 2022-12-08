using Newtonsoft.Json;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Entities
{
    public class SchedulerDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("hostName")]
        public string HostName { get; set; }

        [JsonProperty("tasks")]
        public ISet<APITaskDto> Tasks { get; set; }

        [JsonProperty("defaultHost")]
        public bool DefaultHost { get; set; }
    }
}
