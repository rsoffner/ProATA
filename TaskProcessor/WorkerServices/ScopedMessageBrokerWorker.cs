using ProATA.SharedKernel.Enums;
using System.Text.Json;
using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.MessageBrokers.Subscribers;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;

namespace TaskProcessor.WorkerServices
{
    public sealed class ScopedMessageBrokerWorker : IScopedMessageBrokerWorker
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITaskRepository _taskRepository;
        private readonly SubscriberBase _subscriber;
        private readonly IConfiguration _configuration;
        private readonly TaskProcessorManager _taskProcessorManager;

        public ScopedMessageBrokerWorker(IServiceProvider serviceProvider, ITaskRepository taskRepository, IConfiguration configuration, TaskProcessorManager taskProcessorManager)
        {
            _serviceProvider = serviceProvider;
            _taskProcessorManager = taskProcessorManager;
            _taskRepository = taskRepository;
            _configuration = configuration;
            _subscriber = MessageBrokerSubscriberFactory.Create(MessageBrokerType.RabbitMq, _configuration);
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                _subscriber.Subscribe(async (subs, messageReceivedEventArgs) =>
                {
                    var body = messageReceivedEventArgs.ReceivedMessage.Body;
                    CommandMessage commandMessage = JsonSerializer.Deserialize<CommandMessage>(body);

                    if (commandMessage != null)
                    {
                        var task = await _taskRepository.GetTask(commandMessage.TaskId, stoppingToken);
                        if (task != null)
                        {
                            switch (commandMessage.Command)
                            {
                                case TaskCommand.Run:
                                    await _taskProcessorManager.RunTask(task);
                                    break;
                            }

                            await subs.Acknowledge(messageReceivedEventArgs.AcknowledgeToken);
                        }
                    }
                });
            }
        }
    }
}
