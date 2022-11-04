using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface ITaskRepository
    {
        Task<APITask> GetTask(Guid id, CancellationToken cancellationToken = default);
    }
}
