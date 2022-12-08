using ApiManager.Services.SignalProcessor.Processors;
using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.MessageBrokers.Publishers;
using TaskProcessing.Core.MessageBrokers.Subscribers;

namespace ApiManager.Services.SignalProcessor
{
    public class SignalProcessorManager : IDisposable
    {
        private bool _disposed;
        private readonly IConfiguration _configuration;

        private SubscriberBase _subscriberEventMessage;
        private PublisherCommandMessageBase _publisherCommandMessage;

        public SignalProcessorManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task StartListening(Func<EventMessage, Task> onMessageCallback)
        {
            _subscriberEventMessage = new SubscriberRabbitMq();
            await _subscriberEventMessage.Initialize(_configuration["MessageBroker:MessageBrokerConnectionString"],
                _configuration["MessageBroker:MessageBrokerCommandTopic"], _configuration["MessageBroker:MessageBrokerCommandQueue"] + "." + Environment.MachineName);
            _subscriberEventMessage.Subscribe(OnEventMessageReceived, onMessageCallback);
        }

        private async Task OnEventMessageReceived(SubscriberBase subscriberEventMessage, MessageReceivedEventArgs messageReceivedEventArgs, Func<EventMessage, Task> onMessageCallback)
        {
            var message = messageReceivedEventArgs.Message;
            var eventMessage = MessageProcessor.DeserializeMessage(message);
            await onMessageCallback(eventMessage);
            await subscriberEventMessage.Acknowledge(messageReceivedEventArgs.AcknowledgeToken);
        }


        public async Task PublishCommandMessage(CommandMessage commandMessage)
        {
            _publisherCommandMessage = new PublisherCommandMessageRabbitMq(_configuration["MessageBroker:MessageBrokerConnectionString"], _configuration["MessageBroker:MessageBrokerCommandTopic"]);
            await _publisherCommandMessage.Publish(commandMessage);
        }


        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _subscriberEventMessage?.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
