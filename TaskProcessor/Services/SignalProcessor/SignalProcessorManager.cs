using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.MessageBrokers.Subscribers;
using TaskProcessor.Services.SignalProcessor.Processors;

namespace TaskProcessor.Services.SignalProcessor
{
    public class SignalProcessorManager : IDisposable
    {
        private bool _disposed;
        private readonly IConfiguration _configuration;

        private SubscriberBase _subscriberEventMessage;

        public SignalProcessorManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task StartListening(Func<CommandMessage, Task> onMessageCallback)
        {
            _subscriberEventMessage = new SubscriberRabbitMq();
            await _subscriberEventMessage.Initialize(_configuration["MessageBroker:MessageBrokerConnectionString"],
                _configuration["MessageBroker:MessageBrokerCommandTopic"], _configuration["MessageBroker:MessageBrokerCommandQueue"] + "." + Environment.MachineName);
            _subscriberEventMessage.Subscribe(OnEventMessageReceived, onMessageCallback);
        }

        private async Task OnEventMessageReceived(SubscriberBase subscriberEventMessage, MessageReceivedEventArgs messageReceivedEventArgs, Func<CommandMessage, Task> onMessageCallback)
        {
            var message = messageReceivedEventArgs.Message;
            var commandMessage = MessageProcessor.DeserializeMessage(message);
            await onMessageCallback(commandMessage);
            await subscriberEventMessage.Acknowledge(messageReceivedEventArgs.AcknowledgeToken);
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
