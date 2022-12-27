using TaskProcessing.Core.MessageBrokers.Models;

namespace askProcessing.Core.Services.SignalProcessor
{
    public interface ISignalProcessorManager
    {
        Task StartListening(Func<CommandMessage, Task> onMessageCallback);
    }
}