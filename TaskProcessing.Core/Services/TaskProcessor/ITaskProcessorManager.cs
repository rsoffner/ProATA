using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Services.TaskProcessor
{
    public interface ITaskProcessorManager
    {
        Task RunTask(APITask task);
    }
}