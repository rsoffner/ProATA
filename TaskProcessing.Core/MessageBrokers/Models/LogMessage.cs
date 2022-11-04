using ProATA.SharedKernel.Enums;

namespace TaskProcessing.Core.MessageBrokers.Models
{
    public class LogMessage
    {
        public ErrorType ErrorType { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Detail { get; set; }
        public string Url { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Duration { get; set; }
    }
}
