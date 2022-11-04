namespace TaskProcessing.Core.MessageBrokers.Models
{
    public sealed class BrokerMessage
    {
        public byte[] Body { get; }
        public string MessageId { get; }
        public string ContentType { get; }
        public DateTime CreationDateTime { get; }

        public BrokerMessage(byte[] body, string messageId, string contentType, DateTime creationDateTime)
        {
            Body = body;
            MessageId = messageId;
            ContentType = contentType;
            CreationDateTime = creationDateTime;
        }
    }
}
