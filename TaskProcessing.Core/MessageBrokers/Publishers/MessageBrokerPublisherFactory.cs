﻿using Microsoft.Extensions.Configuration;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.Exceptions;

namespace TaskProcessing.Core.MessageBrokers.Publishers
{
    public static class MessageBrokerPublisherFactory
    {
        public static PublisherCommandMessageBase Create(MessageBrokerType messageBrokerType, IConfiguration configuration)
        {
            switch (messageBrokerType)
            {
                case MessageBrokerType.RabbitMq:
                    string brokerConnectionString = configuration["MessageBroker:MessageBrokerConnectionString"];
                    string brokerTopic  = configuration["MessageBroker:MessageBrokerCommandTopic"]; 
                    return new PublisherCommandMessageRabbitMq(brokerConnectionString, brokerTopic);
            }

            throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: {messageBrokerType}, is not supported yet");
        }
    }
}
