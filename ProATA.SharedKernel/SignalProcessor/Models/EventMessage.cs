﻿using ProATA.SharedKernel.Interfaces;
using System;

namespace ProATA.SharedKernel.SignalProcessor
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