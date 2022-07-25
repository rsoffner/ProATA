using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface ITaskRepository
    {
        APITask GetTask(Guid id);
    }
}
