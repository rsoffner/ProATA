using RabbitMQ.Client;
using TaskProcessing.Core.MessageBrokers.Models;

namespace TaskProcessing.Core.MessageBrokers.Publishers
{
    public class PublisherCommandMessageRabbitMq : PublisherCommandMessageBase
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _topic;

        public PublisherCommandMessageRabbitMq(string connectionString, string topic)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _topic = topic;
        }

        protected override Task PublishCore(CommandMessage commandMessage)
        {

            var basicProperties = InitializeBasicProperties(_channel.CreateBasicProperties(), commandMessage.TaskId.ToString());
            byte[] messageBody = SerializeMessage(commandMessage);
            _channel.BasicPublish(_topic, routingKey:commandMessage.Destination, basicProperties, messageBody);

            return Task.CompletedTask;
        }

        private static IBasicProperties InitializeBasicProperties(IBasicProperties basicProperties, string id)
        {
            basicProperties.Persistent = true;

            var propertiesDictionary = new Dictionary<string, object>();
            basicProperties.Headers = propertiesDictionary;
            propertiesDictionary.Add("message_id", id);
            propertiesDictionary.Add("content_type", "application/json");
            propertiesDictionary.Add("creation_date", DateTimeOffset.UtcNow.ToString("o"));

            return basicProperties;
        }
    }
}
