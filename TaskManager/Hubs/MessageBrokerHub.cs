﻿using Microsoft.AspNetCore.SignalR;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.Services;

namespace TaskManager.Hubs
{
    public sealed class MessageBrokerHub : Hub
    {
        private readonly SignalProcessorManager _signalProcessorManager;

        public MessageBrokerHub(SignalProcessorManager signalProcessorManager)
        {
            _signalProcessorManager = signalProcessorManager;
        }

        public async Task CommandReceived(Guid taskId, TaskCommand command)
        {
            await _signalProcessorManager.PublishCommandMessage(new CommandMessage(Guid.NewGuid().ToString("N"), taskId, command, DateTime.UtcNow));
        }
    }
}
