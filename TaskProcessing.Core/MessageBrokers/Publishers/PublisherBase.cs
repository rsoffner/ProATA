using TaskProcessing.Core.MessageBrokers.Models;

namespace TaskProcessing.Core.MessageBrokers.Publishers
{
    public abstract class PublisherBase : IDisposable
    {
        public Task Publish(BrokerMessage message)
        {
            return PublishCore(message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract Task PublishCore(BrokerMessage message);
        protected abstract void Dispose(bool disposing);
    }
}
