using MediatR;
using ProATA.SharedKernel.Enums;

namespace TaskProcessing.Core.Events
{
    public class TaskStateChangedEvent : INotification
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
