using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services;
using TaskProcessing.Core.Services.TaskProcessor;

namespace TaskProcessor.WorkerServices
{
    public sealed class MessageBrokerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MessageBrokerWorker(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<MessageBrokerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            try
            {
                await DoWorkAsync(stoppingToken);

                var delay = _configuration["ApiLogService:Interval"];
                // await Task.Delay(TimeSpan.FromMinutes(Convert.ToInt32(delay)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }
            //}
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            IScopedMessageBrokerWorker scopedMessageBrokerWorker = scope.ServiceProvider.GetRequiredService<IScopedMessageBrokerWorker>();

            await scopedMessageBrokerWorker.DoWorkAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {

            await base.StopAsync(cancellationToken);
        }
    }
}
