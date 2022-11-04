using Microsoft.AspNetCore.SignalR;
using ProATA.SharedKernel.Enums;
using System.Text;
using System.Text.Json;
using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.MessageBrokers.Publishers;

namespace APIManager.Hubs
{
    public class MessageBrokerHub : Hub
    {
        private readonly PublisherBase _publisher;

        public MessageBrokerHub(IConfiguration configuration)
        {
           _publisher = MessageBrokerPublisherFactory.Create(MessageBrokerType.RabbitMq, configuration);
        }

        public async Task CommandReceived(Guid taskId, TaskCommand command)
        {
            var commandMessage = new CommandMessage(taskId, command);
            var json = JsonSerializer.Serialize(commandMessage);
            var messageBytes = Encoding.UTF8.GetBytes(json);
            var message = new BrokerMessage(messageBytes, Guid.NewGuid().ToString("N"), "application/json", DateTime.Now);
            await _publisher.Publish(message);
        }
    }
}
