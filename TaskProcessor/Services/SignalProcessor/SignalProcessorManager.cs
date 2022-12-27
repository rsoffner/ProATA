using askProcessing.Core.Services.SignalProcessor;
using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.MessageBrokers.Subscribers;
using TaskProcessor.Services.SignalProcessor.Processors;

namespace TaskProcessor.Services.SignalProcessor
{
    public class SignalProcessorManager : ISignalProcessorManager
    {
        private readonly IConfiguration _configuration;
        private SubscriberBase _subscriber;

        public SignalProcessorManager(IConfiguration configuration, SubscriberBase subscriber)
        {
            _configuration = configuration;
            _subscriber = subscriber;
        }

        public async Task StartListening(Func<CommandMessage, Task> onMessageCallback)
        {
            await _subscriber.Initialize(_configuration["MessageBroker:MessageBrokerConnectionString"],
                _configuration["MessageBroker:MessageBrokerCommandTopic"], _configuration["MessageBroker:MessageBrokerCommandQueue"] + "." + Environment.MachineName);
            _subscriber.Subscribe(OnEventMessageReceived, onMessageCallback);
        }

        public Task StartListening(Func<EventMessage, Task> onMessageCallback)
        {
            throw new NotImplementedException();
        }

        private async Task OnEventMessageReceived(SubscriberBase subscriberEventMessage, MessageReceivedEventArgs messageReceivedEventArgs, Func<CommandMessage, Task> onMessageCallback)
        {
            var message = messageReceivedEventArgs.Message;
            var commandMessage = MessageProcessor.DeserializeMessage(message);
            await onMessageCallback(commandMessage);
            await subscriberEventMessage.Acknowledge(messageReceivedEventArgs.AcknowledgeToken);
        }
    }
}
