using askProcessing.Core.Services.SignalProcessor;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Services.TaskProcessor;
using TaskProcessing.Core.Services.TaskScheduler;

namespace TaskProcessor.WorkerServices
{
    public sealed class ScopedMessageBrokerWorker : IScopedMessageBrokerWorker
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISignalProcessorManager _signalProcessorManager;
        private readonly ITaskProcessorManager _taskProcessorManager;
        private readonly ITaskSchedulerManager _taskSchedulerManager;
        private readonly ILogger<ScopedMessageBrokerWorker> _logger;

        public ScopedMessageBrokerWorker(IServiceProvider serviceProvider,
            ITaskProcessorManager taskProcessorManager, 
            ISignalProcessorManager signalProcessorManager, 
            ITaskSchedulerManager taskSchedulerManager,
            ILogger<ScopedMessageBrokerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _taskProcessorManager = taskProcessorManager;
            _signalProcessorManager = signalProcessorManager;
            _taskSchedulerManager = taskSchedulerManager;
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            _taskSchedulerManager.StartScheduler();

            await _signalProcessorManager.StartListening(async commandMessage =>
            {
                switch (commandMessage.Command)
                {
                    case TaskCommand.Run:
                        APITask task = _taskSchedulerManager.GetTask(commandMessage.TaskId);
                        await _taskProcessorManager.RunTask(task);
                        break;
                }
            });
        }
    }
}
