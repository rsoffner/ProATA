﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace TaskProcessing.Core.Services
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public abstract class SignalProcessorTechnicalBaseException : SignalProcessorBaseException
    {
        protected SignalProcessorTechnicalBaseException() { }
        protected SignalProcessorTechnicalBaseException(string message) : base(message) { }
        protected SignalProcessorTechnicalBaseException(string message, Exception inner) : base(message, inner) { }
        protected SignalProcessorTechnicalBaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
