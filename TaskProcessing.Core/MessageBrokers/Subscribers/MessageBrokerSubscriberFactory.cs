using Microsoft.Extensions.Configuration;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.Exceptions;

namespace TaskProcessing.Core.MessageBrokers.Subscribers
{
    public static class MessageBrokerSubscriberFactory
    {
        public static SubscriberBase Create(MessageBrokerType messageBrokerType, IConfiguration configuration)
        {
            SubscriberBase subscriber = null;
            string connectionString = null;

            switch (messageBrokerType)
            {
                case MessageBrokerType.RabbitMq:
                    subscriber = new SubscriberRabbitMq();
                    connectionString = configuration["MessageBroker:MessageBrokerConnectionString"]; // brokerConnectionStringRabbitMq;
                    break;
                default:
                    throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: {messageBrokerType}, is not supported yet");
            }

            var topic = configuration["MessageBroker:MessageBrokerCommandTopic"];
            var queue = configuration["MessageBroker:MessageBrokerCommandQueue"];

            subscriber.Initialize(connectionString, topic, queue);
            return subscriber;
        }
    }
}
