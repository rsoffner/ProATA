using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace ProATA.SharedKernel.SignalProcessor
{
    internal sealed class SubscriberServiceBus : SubscriberBase
    {
        private bool _disposed;
        // private readonly ServiceBusClient _subscriptionClient;
        private ServiceBusProcessor _processor;

        protected override async Task InitializeCore(string connectionString, string topicName, string queueName)
        {
            var managementClient = new ServiceBusClient(connectionString);
            
            _processor = managementClient.CreateProcessor(topicName, queueName, new ServiceBusProcessorOptions()
            {
                AutoCompleteMessages = false
            });
        }

        protected override void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Func<EventMessage, Task>, Task> receiveCallback, Func<EventMessage, Task> onMessageCallback)
        {

            _processor.ProcessMessageAsync += async (sbMessage) =>
            {
                sbMessage.CancellationToken.ThrowIfCancellationRequested();

                var messageReceivedEventArgs = new MessageReceivedEventArgs(
                        new Message(sbMessage.Message.Body.ToArray(),
                                    Guid.NewGuid().ToString("N"), "application/json"),
                                    (string)sbMessage.Message.ApplicationProperties["LockToken"],
                                    sbMessage.CancellationToken);

                await receiveCallback(this, messageReceivedEventArgs, onMessageCallback);
            };
            _processor.ProcessErrorAsync += _processor_ProcessErrorAsync;

        }

        private Task _processor_ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            throw new SignalProcessorMessageReceiveException(args.Exception.Message, args.Exception); ;
        }

        protected override Task AcknowledgeCore(string acknowledgetoken)
        {
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed && _processor != null)
            {
                _processor.CloseAsync().ContinueWith(continuationAction =>
                {
                    continuationAction.Wait();
                });

                _disposed = true;
            }
        }       
    }
}
