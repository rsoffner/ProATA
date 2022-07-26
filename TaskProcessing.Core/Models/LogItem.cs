﻿using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public class LogItem : Entity<int>
    {
        public virtual DateTime TimeStamp { get; protected set; }
        public virtual int Priority { get; protected set; }
        public virtual string Message { get; protected set; }
        public virtual string PriorityName { get; protected set; }
        public virtual string Url { get; protected set; }
        public virtual string Detail { get; protected set; }
        public virtual bool Acknowledged { get; set; }
        public virtual int UserId { get; set; }
        public virtual Guid TaskId { get; set; }
        public virtual double Duration { get; set; }
        public virtual string Source { get; set; }

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
