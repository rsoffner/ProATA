using ProATA.SharedKernel.Interfaces;

namespace TaskProcessing.Core.Events
{
    public class TaskCreatedEvent : IDomainEvent
    {
        public TaskCreatedEvent(Guid taskId, string title)
        {
            TaskId = taskId;
            Title = title;
        }

        public Guid TaskId { get; private set; }
        public string Title { get; private set; }
        public DateTime DateTimeEventOccurred => DateTime.UtcNow;
    }
}
