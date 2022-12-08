using Newtonsoft.Json;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.GraphQL
{
    public class SchedulersResponse
    {
        [JsonProperty("schedulers")]
        public IEnumerable<SchedulerDto> Schedulers { get; set; }
    }
}
