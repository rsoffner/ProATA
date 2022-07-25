﻿namespace ProATA.SharedKernel.SignalProcessor
{
    public sealed class MessageBrokerSettings
    {
        public string MessageBrokerConnectionString { get; }

        public MessageBrokerType MessageBrokerType { get; }

        public MessageBrokerSettings(string messageBrokerConnectionString, MessageBrokerType messageBrokerType)
        {
            MessageBrokerConnectionString = messageBrokerConnectionString;
            MessageBrokerType = messageBrokerType;
        }
    }
}
