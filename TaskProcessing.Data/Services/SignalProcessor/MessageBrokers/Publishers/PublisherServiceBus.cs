using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;
//using ServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace TaskProcessing.Data.Services
{
    internal class PublisherServiceBus<TMessage> : PublisherBase<TMessage>
    {
        private bool _disposed;
        //private readonly TopicClient _topicClient;
        private readonly ServiceBusClient _topicClient;
        private readonly ServiceBusSender _sender;

        public PublisherServiceBus(string connectionString, string topicName)
        {
            //_topicClient = new TopicClient(connectionString, topicName);
            _topicClient = new ServiceBusClient(connectionString);
            _sender = _topicClient.CreateSender(topicName);
        }

        protected override async Task PublishCore(TMessage message, string id)
        {
            byte[] messageBody = SerializeMessage(message);
            var serviceBusMessage = InitializeMessageProperties(messageBody, id);
            //await _topicClient.SendAsync(serviceBusMessage);
            await _sender.SendMessageAsync(serviceBusMessage);
        }

        private static ServiceBusMessage InitializeMessageProperties(byte[] messageBody, string id)
        {
            var serviceBusMessage = new ServiceBusMessage(messageBody)
            {
                MessageId = id,
                ContentType = "application/json"
            };
            serviceBusMessage.ApplicationProperties.Add("CreationDate", DateTimeOffset.UtcNow.ToString("o"));

            return serviceBusMessage;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                /*
                _topicClient.CloseAsync().ContinueWith(continuationTask =>
                {
                    continuationTask.Wait();
                });
                */
                _sender.CloseAsync().ContinueWith(continuationTask =>
                {
                    continuationTask.Wait();
                });

                _disposed = true;
            }
        }
    }
}
