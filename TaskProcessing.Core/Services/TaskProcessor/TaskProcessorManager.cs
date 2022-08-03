using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using TaskProcessing.Core.Interfaces;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Services.TaskProcessor
{
    public sealed class TaskProcessorManager : IDisposable
    {
        private bool _disposed;
        //private readonly PublisherBase<Message> _messageBrokerPublisher;
        private readonly SignalProcessorManager _signalProcessorManager;

        public TaskProcessorManager(SignalProcessorManager signalProcessorManager)
        {
            // _messageBrokerPublisher = messageBrokerPublisher;
            _signalProcessorManager = signalProcessorManager;
        }

        public async Task RunTask(APITask task)
        {
            TextInfo info = new CultureInfo("en-US", false).TextInfo;
            string baseClassname = Regex.Replace(info.ToTitleCase(task.Title), @"\s+", "");

            string strategyName = "TaskProcessing.Core.Strategies." + baseClassname;
            Type strategyType = Type.GetType(strategyName + ", TaskProcessing.Core");
            if (strategyType != null)
            {
                var strategy = Activator.CreateInstance(strategyType) as IRunStrategy;
                if (strategy != null)
                {
                    task.SetRunStrategy(strategy);
                    await task.Run();
                    await task.End();
                    foreach (var domainEvent in task.Events)
                    {
                        var json = JsonSerializer.Serialize(domainEvent);
                        var messageBytes = Encoding.UTF8.GetBytes(json);
                        var message = new Message(messageBytes, Guid.NewGuid().ToString("N"), "application/json");
                        await _signalProcessorManager.PublishMessage(message);
                    }
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _signalProcessorManager?.Dispose();
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
