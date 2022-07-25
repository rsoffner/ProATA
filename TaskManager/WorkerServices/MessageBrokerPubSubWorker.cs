using Microsoft.AspNetCore.SignalR;
using ProATA.SharedKernel.SignalProcessor;
using TaskManager.Hubs;

namespace TaskManager.WorkerServices
{
    public sealed class MessageBrokerPubSubWorker : BackgroundService
    {
        private readonly SignalProcessorManager _signalProcessorManager;
        private readonly IHubContext<MessageBrokerHub> _messageBrokerHubContext;

        public MessageBrokerPubSubWorker(IHubContext<MessageBrokerHub> messageBrokerHubContext, SignalProcessorManager signalProcessorManager)
        {
            _messageBrokerHubContext = messageBrokerHubContext;
            _signalProcessorManager = signalProcessorManager;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await _signalProcessorManager.StartListening(async eventMessage =>
            {
                await _messageBrokerHubContext.Clients.All.SendAsync("onMessageReceived", eventMessage, stoppingToken);
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _signalProcessorManager.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
