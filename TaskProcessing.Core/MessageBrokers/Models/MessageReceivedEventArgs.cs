namespace TaskProcessing.Core.MessageBrokers.Models
{
    public sealed class MessageReceivedEventArgs : EventArgs
    {
        public BrokerMessage Message { get; }
        public string AcknowledgeToken { get; }
        public CancellationToken CancellationToken { get; }

        public MessageReceivedEventArgs(BrokerMessage message, string acknowledgeToken, CancellationToken cancellationToken)
        {
            Message = message;
            AcknowledgeToken = acknowledgeToken;
            CancellationToken = cancellationToken;
        }
    }
}
