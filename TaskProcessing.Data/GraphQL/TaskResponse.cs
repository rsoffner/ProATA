using Newtonsoft.Json;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.GraphQL
{
    public class TaskResponse
    {
        [JsonProperty("task")]
        public APITaskDto Task { get; set; }
    }
}
