using ProATA.SharedKernel.Enums;
using System;

namespace TaskProcessing.Core.MessageBrokers.Models
{
    public sealed class CommandMessage
    {
        public Guid TaskId { get; set; }
        public TaskCommand Command { get; }
        public string Destination { get; }
        public DateTime CreatedDateTime => DateTime.UtcNow;

        public CommandMessage(string destination, Guid taskId, TaskCommand command)
        {
            Command = command;
            TaskId = taskId;
            Destination = destination;
        }
    }
}
