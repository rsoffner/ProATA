using ApiManager.Services.SignalProcessor;
using Microsoft.AspNetCore.SignalR;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Hubs
{
    public class MessageBrokerHub : Hub
    {
        private readonly IConfiguration _configuration;
        private readonly SignalProcessorManager _signalProcessorManager;
        private readonly ISchedulerRepository _schedulerRepository;

        public MessageBrokerHub(IConfiguration configuration, ISchedulerRepository schedulerRepository) 
        {
            _configuration = configuration;
            _signalProcessorManager = new SignalProcessorManager(configuration);
            _schedulerRepository = schedulerRepository;
        }

        public async Task CommandReceived(Guid schedulerId, Guid taskId, TaskCommand command)
        {
            var scheduler = _schedulerRepository.GetById(schedulerId);

            if (scheduler != null)
            {
                await _signalProcessorManager.PublishCommandMessage(new CommandMessage(scheduler.HostName, taskId, command));
            }
        }


    }
}
