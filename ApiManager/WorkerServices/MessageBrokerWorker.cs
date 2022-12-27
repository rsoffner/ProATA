using ApiManager.Hubs;
using ApiManager.Services.SignalProcessor;
using askProcessing.Core.Services.SignalProcessor;
using Microsoft.AspNetCore.SignalR;

namespace ApiManager.WorkerServices
{
    public class MessageBrokerWorker : BackgroundService
    {
        private readonly ISignalProcessorManager _signalProcessorManager;
        private readonly IHubContext<MessageBrokerHub> _hubContext;
        private readonly IConfiguration _configuration;

        public MessageBrokerWorker(IHubContext<MessageBrokerHub> hubContext, IConfiguration configuration, ISignalProcessorManager signalProcessorManager)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _signalProcessorManager= signalProcessorManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _signalProcessorManager.StartListening(async eventMessage =>
            {

            });
        }
    }
}
