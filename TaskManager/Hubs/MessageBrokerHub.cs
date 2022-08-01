using Microsoft.AspNetCore.SignalR;
using ProATA.SharedKernel.Enums;
using ProATA.SharedKernel.SignalProcessor;

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

        public Task ReceiveMessage(string message)
        {
            return Task.CompletedTask;
        }
    }
}
