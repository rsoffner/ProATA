using Newtonsoft.Json;
using System.Text;
using TaskProcessing.Core.Exceptions;
using TaskProcessing.Core.MessageBrokers.Models;

namespace TaskProcessing.Core.MessageBrokers.Subscribers
{
    public abstract class SubscriberBase
    {
        public Task Initialize(string connectionString, string topicName, string queueName)
        {
            return InitializeCore(connectionString, topicName, queueName);
        }

        public void Subscribe(Func<SubscriberBase, MessageReceivedEventArgs, Func<EventMessage, Task>, Task> receiveCallback, Func<EventMessage, Task> onMessageCallback)
        {
            SubscribeCore(receiveCallback, onMessageCallback);
        }

        public void Subscribe(Func<SubscriberBase, MessageReceivedEventArgs, Func<CommandMessage, Task>, Task> receiveCallback, Func<CommandMessage, Task> onMessageCallback)
        {
            SubscribeCore(receiveCallback, onMessageCallback);
        }

        public Task Acknowledge(string acknowledgetoken)
        {
            return AcknowledgeCore(acknowledgetoken);
        }

        public static T Deserialize<T>(byte[] jsonBytes)
        {
            var jsonString = Encoding.UTF8.GetString(jsonBytes);

            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonException e)
            {
                throw new MessageDeserializationFailedException($@"Exception occurred while attempting to deserialize JSON Message body to target of Type: {typeof(T).Name}. JSON Body received: {jsonString}. The Original Exception type is: {e.GetType().Name}. Original Exception message: {e.Message}", e);
            }
        }

        protected abstract Task AcknowledgeCore(string acknowledgetoken);
        protected abstract Task InitializeCore(string connectionString, string topicName, string queueName);
        protected abstract void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Func<EventMessage, Task>, Task> receiveCallback, Func<EventMessage, Task> onMessageCallback);
        protected abstract void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Func<CommandMessage, Task>, Task> receiveCallback, Func<CommandMessage, Task> onMessageCallback);
    }
}
