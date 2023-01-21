using TaskProcessing.Core.Models;
using TaskProcessing.Core.Services.TaskProcessor;
using System.Net;
using TaskProcessing.Core.Models.ValueObjects;

namespace TaskProcesser.Services.TaskProcessor
{
    public class TaskProcessorManager : ITaskProcessorManager
    {
        private readonly IConfiguration _configuration;
        
        public TaskProcessorManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TaskResponse> RunTask(APITask task)
        {
            try
            {
                await task.Run();
                await task.End();

                return new TaskResponse(HttpStatusCode.OK, $"Task {task.Title} was executed");
            }
            catch (Exception ex)
            {
                return new TaskResponse(HttpStatusCode.InternalServerError, $"Task {task.Title} throw an error: {ex.Message}");
            }
        }
    }
}
