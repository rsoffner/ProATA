using ProATA.SharedKernel.Enums;
using ProATA.SharedKernel.Interfaces;

namespace TaskProcessing.Core.Events
{
    public class TaskStateChangedEvent : IDomainEvent
    {
        public Guid TaskId { get; private set; }
        public TaskState State { get; private set; }
        public DateTime DateTimeEventOccurred => DateTime.UtcNow;

        public TaskStateChangedEvent(Guid taskId, TaskState state)
        {
            TaskId = taskId;
            State = state;
        }   
    }
}
