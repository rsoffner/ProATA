using Microsoft.Azure.ServiceBus;
using SbMessage = Microsoft.Azure.ServiceBus.Message;

namespace TaskProcessing.Core.MessageBrokers.Publishers
{
    public sealed class PublisherServiceBus : PublisherBase
    {
        private bool _disposed;
        private readonly TopicClient _topicClient;

        public PublisherServiceBus(string connectionString, string topic)
        {
            _topicClient = new TopicClient(connectionString, topic);
        }

        protected override async Task PublishCore(ProATA.SharedKernel.SignalProcessor.Message message)
        {
            var sbMessage = new SbMessage(message.Body);
            sbMessage.ContentType = message.ContentType;
            sbMessage.MessageId = message.MessageId;
            await _topicClient.SendAsync(sbMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (_topicClient != null)
                {
                    _topicClient.CloseAsync().ContinueWith(continuationTask =>
                    {
                        continuationTask.Wait();
                    });
                }

                _disposed = true;
            }
        }
    }
}
