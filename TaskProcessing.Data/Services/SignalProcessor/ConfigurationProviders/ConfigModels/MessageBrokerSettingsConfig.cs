namespace TaskProcessing.Data.Services
{
    internal sealed class MessageBrokerSettingsConfig
    {
        public string MessageBrokerConnectionString { get; set; }

        public string MessageBrokerType { get; set; }
    }
}
