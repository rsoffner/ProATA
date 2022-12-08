using Newtonsoft.Json;

namespace TaskProcessing.Data.Entities
{
    public class APITaskDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("scheduler")]
        public SchedulerDto Scheduler { get; set; }

        [JsonProperty("schedules")]
        public IList<ScheduleDto> Schedules { get; set; }
    }
}
