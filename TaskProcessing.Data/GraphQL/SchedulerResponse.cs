using Newtonsoft.Json;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.GraphQL
{
    public class SchedulerResponse
    {
        [JsonProperty("scheduler")]
        public SchedulerDto Scheduler { get; set; }
    }
}
