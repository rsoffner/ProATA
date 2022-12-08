using ProATA.SharedKernel;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface ITaskRepository
    {
        APITask GetTask(Guid id);
        IEnumerable<APITask> GetAllTasks();
        DatabaseResponse<APITask> GetTasks(int page, int pageSize);

        DatabaseResponse<APITask> GetTasksByScheduler(Guid schedulerId, int page, int pageSize);
        void AddTask(APITask task);

    }
}
