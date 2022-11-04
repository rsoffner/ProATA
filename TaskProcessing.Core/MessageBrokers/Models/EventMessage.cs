using ProATA.SharedKernel.Interfaces;

namespace TaskProcessing.Core.MessageBrokers.Models
{
    public sealed class EventMessage : IDomainEvent
    {
        public EventMessage(Guid taskId, string message)
        {
            TaskId = taskId;
            Message = message;
        }

        public Guid TaskId { get; private set; }
        public string Message { get; private set; }
        public DateTime DateTimeEventOccurred => DateTime.UtcNow;
    }
}
