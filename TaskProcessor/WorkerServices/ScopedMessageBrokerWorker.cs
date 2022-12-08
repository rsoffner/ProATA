using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;
using TaskProcessor.Services.SignalProcessor;

namespace TaskProcessor.WorkerServices
{
    public sealed class ScopedMessageBrokerWorker : IScopedMessageBrokerWorker
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITaskRepository _taskRepository;
        private readonly SignalProcessorManager _signalProcessorManager;
        private readonly IConfiguration _configuration;
        private readonly TaskProcessorManager _taskProcessorManager;

        public ScopedMessageBrokerWorker(IServiceProvider serviceProvider, ITaskRepository taskRepository, IConfiguration configuration, TaskProcessorManager taskProcessorManager)
        {
            _serviceProvider = serviceProvider;
            _taskProcessorManager = taskProcessorManager;
            _taskRepository = taskRepository;
            _configuration = configuration;
            _signalProcessorManager = new SignalProcessorManager(configuration);
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                await _signalProcessorManager.StartListening(async commandMessage =>
                {
                    var task = _taskRepository.GetTask(commandMessage.TaskId);
                    if (task != null)
                    {
                        switch (commandMessage.Command)
                        {
                            case TaskCommand.Run:
                                await _taskProcessorManager.RunTask(task);
                                break;
                        }
                    }

                });
            }
        }
    }
}
