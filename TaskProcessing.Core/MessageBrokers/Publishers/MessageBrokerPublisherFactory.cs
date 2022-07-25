using ProATA.SharedKernel.SignalProcessor;
using TaskProcessing.Core.Exceptions;

namespace TaskProcessing.Core.MessageBrokers.Publishers
{
    public static class MessageBrokerPublisherFactory
    {
        const string brokerConnectionStringRabbitMq = "amqp://SAAPI:SA32api@ap-dev-01/apitaskmanagement.vhost";
        const string brokerConnectionStringServiceBus = "<Your ServiceBus Connection string>";
        const string titleTopic = "task.events.topic";

        public static PublisherBase Create(MessageBrokerType messageBrokerType)
        {
            switch (messageBrokerType)
            {
                case MessageBrokerType.RabbitMq:
                    return new PublisherRabbitMq(brokerConnectionStringRabbitMq, titleTopic);
                        
                case MessageBrokerType.ServiceBus:
                    return new PublisherServiceBus(brokerConnectionStringServiceBus, titleTopic);
            }

            throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: {messageBrokerType}, is not supported yet");
        }
    }
}
