using RabbitMQ.Client;
using TaskProcessing.Core.MessageBrokers.Models;

namespace TaskProcessing.Core.MessageBrokers.Publishers
{
    public class PublisherRabbitMq : PublisherBase
    {
        private bool _disposed;
        private readonly IConnection _connection;
        private readonly string _topic;

        public PublisherRabbitMq(string connectionString, string topic)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
            };

            _connection = connectionFactory.CreateConnection();
            _topic = topic;
        }

        protected override Task PublishCore(BrokerMessage message)
        {
            using (var channel = _connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = message.ContentType;
                properties.MessageId = message.MessageId;

                var propertiesDictionary = new Dictionary<string, object>();
                properties.Headers = propertiesDictionary;
                channel.BasicPublish(_topic, routingKey: string.Empty, properties, message.Body);
                _connection.Close();
            }

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
