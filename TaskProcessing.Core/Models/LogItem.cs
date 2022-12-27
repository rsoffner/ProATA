using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public class LogItem : Entity<int>
    {
        public DateTime TimeStamp { get; protected set; }
        public int Priority { get; protected set; }
        public string Message { get; protected set; }
        public string PriorityName { get; protected set; }
        public string Url { get; protected set; }
        public string Detail { get; protected set; }
        public bool Acknowledged { get; set; }
        public int UserId { get; set; }
        public Guid TaskId { get; set; }
        public double Duration { get; set; }
        public string Source { get; set; }

        public LogItem() { }

        public LogItem(DateTime timeStamp,
            int priority,
            string message,
            string priorityName,
            string url,
            string detail,
            bool acknowledged,
            int userId = -1,
            double duration = 0,
            string source = null)
        {
            TimeStamp = timeStamp;
            Priority = priority;
            Message = message;
            PriorityName = priorityName;
            Url = url;
            Detail = detail;
            Acknowledged = acknowledged;
            UserId = userId;
            Duration = duration;
            Source = source;
        }
    }

}
