using ProATA.SharedKernel.Enums;

namespace TaskManager.Hubs
{
    public interface IMessageBroker
    {
        Task CommandReceived(Guid taskId, TaskCommand command);
    }
}