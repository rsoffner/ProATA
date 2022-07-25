using ProATA.SharedKernel.SignalProcessor;
using TaskProcessing.Core.Exceptions;
using TaskProcessing.Core.MessageBrokers.Publishers;

namespace TaskProcessing.Core.MessageBrokers.Subscribers
{
    public static class MessageBrokerSubscriberFactory
    {
        const string brokerConnectionStringRabbitMq = "amqp://SAAPI:SA32api@ap-dev-01/apitaskmanagement.vhost";
        const string brokerConnectionStringServiceBus = "<Your ServiceBus Connection string>";
        const string commandTopic = "command.events.topic";
        const string commandQueue = "command.events.queue";

        public static SubscriberBase Create(MessageBrokerType messageBrokerType)
        {
            SubscriberBase subscriber = null;
            string connectionString = null;

            switch (messageBrokerType)
            {
                case MessageBrokerType.RabbitMq:
                    subscriber = new SubscriberRabbitMq();
                    connectionString = brokerConnectionStringRabbitMq;
                    break;
                case MessageBrokerType.ServiceBus:
                    subscriber = new SubscriberServiceBus();
                    connectionString = brokerConnectionStringServiceBus;
                    break;
                default:
                    throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: {messageBrokerType}, is not supported yet");
            }

            subscriber.Initialize(connectionString, commandTopic, commandQueue);
            return subscriber;
        }
    }
}
