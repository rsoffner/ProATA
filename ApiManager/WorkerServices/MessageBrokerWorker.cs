using ApiManager.Hubs;
using ApiManager.Services.SignalProcessor;
using Microsoft.AspNetCore.SignalR;

namespace ApiManager.WorkerServices
{
    public class MessageBrokerWorker : BackgroundService
    {
        private readonly SignalProcessorManager _signalProcessorManager;
        private readonly IHubContext<MessageBrokerHub> _hubContext;
        private readonly IConfiguration _configuration;

        public MessageBrokerWorker(IHubContext<MessageBrokerHub> hubContext, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _signalProcessorManager = new SignalProcessorManager(configuration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _signalProcessorManager.StartListening(async eventMessage =>
            {

            });
        }
    }
}
