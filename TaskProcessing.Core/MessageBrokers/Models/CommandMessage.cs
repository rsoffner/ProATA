using ProATA.SharedKernel.Enums;
using System;

namespace TaskProcessing.Core.MessageBrokers.Models
{
    public sealed class CommandMessage
    {
        public Guid  TaskId { get; }
        public TaskCommand Command { get; }
        public DateTime CreatedDateTime => DateTime.UtcNow;

        public CommandMessage(Guid taskId, TaskCommand command)
        {
            TaskId = taskId;
            Command = command;
        }
    }
}
