using ProATA.SharedKernel.Enums;
using System;

namespace TaskProcessing.Data.Services
{
    public sealed class CommandMessage
    {
        public string Id { get; }
        public Guid  TaskId { get; }
        public TaskCommand Command { get; }
        public DateTime CreatedDateTime { get; }

        public CommandMessage(string id, Guid taskId, TaskCommand command, DateTime createdDateTime)
        {
            Id = id;
            TaskId = taskId;
            Command = command;
            CreatedDateTime = createdDateTime;
        }
    }
}
