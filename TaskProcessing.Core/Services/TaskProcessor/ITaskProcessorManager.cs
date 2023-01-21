using TaskProcessing.Core.Models;
using TaskProcessing.Core.Models.ValueObjects;

namespace TaskProcessing.Core.Services.TaskProcessor
{
    public interface ITaskProcessorManager
    {
        Task<TaskResponse> RunTask(APITask task);
    }
}