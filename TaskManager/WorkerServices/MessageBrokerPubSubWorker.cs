using Microsoft.AspNetCore.SignalR;
using TaskManager.Hubs;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services;
using TaskProcessing.Core.Services.TaskProcessor;

namespace TaskManager.WorkerServices
{
    public sealed class MessageBrokerPubSubWorker : BackgroundService
    {
        private readonly SignalProcessorManager _signalProcessorManager;
        private readonly IHubContext<MessageBrokerHub> _messageBrokerHubContext;
        private readonly TaskProcessorManager _taskProcessorManager;
        private readonly ITaskRepository _taskRepository;

        public MessageBrokerPubSubWorker(IHubContext<MessageBrokerHub> messageBrokerHubContext, SignalProcessorManager signalProcessorManager, 
            TaskProcessorManager taskProcessorManager, ITaskRepository taskRepository)
        {
            _messageBrokerHubContext = messageBrokerHubContext;
            _signalProcessorManager = signalProcessorManager;
            _taskProcessorManager = taskProcessorManager;
            _taskRepository = taskRepository;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await _signalProcessorManager.StartListeningForCommands(async commandMessage =>
            {
                await _messageBrokerHubContext.Clients.All.SendAsync("onMessageReceived", commandMessage, stoppingToken);

                
                var task = _taskRepository.GetTask(commandMessage.TaskId);
                if (task != null)
                {
                    switch (commandMessage.Command)
                    {
                        case ProATA.SharedKernel.Enums.TaskCommand.Run:
                            await _taskProcessorManager.RunTask(task);
                            break;
                    }
                }
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _signalProcessorManager.Dispose();
            _taskProcessorManager.Dispose();

            return base.StopAsync(cancellationToken);
        }
    }
}
