using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using TaskProcessing.Core.MessageBrokers.Models;

namespace TaskProcessor.Services.SignalProcessor.Processors
{
    public class MessageProcessor
    {
        private static readonly JsonStringEnumConverter _jsonStringEnumConverter = new JsonStringEnumConverter(namingPolicy: default, allowIntegerValues: false);
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions();

        static MessageProcessor()
        {
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
            _jsonSerializerOptions.Converters.Add(_jsonStringEnumConverter);
        }

        public static CommandMessage DeserializeMessage(BrokerMessage message)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            return Deserialize<CommandMessage>(messageBody);
        }

        private static T Deserialize<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString, _jsonSerializerOptions);
        }
    }
}
