using System;
using System.Diagnostics.CodeAnalysis;

namespace TaskProcessing.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public sealed class AgentMessageReceivedException : Exception
    {
        public AgentMessageReceivedException() { }
        public AgentMessageReceivedException(string message) : base(message) { }
        public AgentMessageReceivedException(string message, Exception inner) : base(message, inner) { }
    }
}