namespace TaskProcessing.Core.MessageBrokers.Models
{
    public sealed class MessageReceivedEventArgs : EventArgs
    {
        public BrokerMessage ReceivedMessage { get; }
        public string AcknowledgeToken { get; }
        public CancellationToken CancellationToken { get; }

        public MessageReceivedEventArgs(BrokerMessage receivedMessage, string acknowledgeToken, CancellationToken cancellationToken)
        {
            ReceivedMessage = receivedMessage;
            AcknowledgeToken = acknowledgeToken;
            CancellationToken = cancellationToken;
        }
    }
}
