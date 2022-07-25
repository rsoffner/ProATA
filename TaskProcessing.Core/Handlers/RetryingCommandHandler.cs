using ProATA.SharedKernel.Interfaces;

namespace TaskProcessing.Core.Handlers
{
    public class RetryingCommandHandler<T> : IHandleCommand<T>
    {
        
        public Task Handle(T command)
        {
            throw new NotImplementedException();
        }
    }
}
