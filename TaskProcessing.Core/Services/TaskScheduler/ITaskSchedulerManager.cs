using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Services.TaskScheduler
{
    public interface ITaskSchedulerManager
    {
        APITask GetTask(Guid taskId);
        void StartScheduler();
    }
}
