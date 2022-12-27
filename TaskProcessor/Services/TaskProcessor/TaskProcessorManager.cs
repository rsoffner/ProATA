using System.Globalization;
using System.Text.RegularExpressions;
using TaskProcessing.Core.Interfaces;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;
using TaskProcessing.Data.Models;

namespace TaskProcesser.Services.TaskProcessor
{
    public class TaskProcessorManager : ITaskProcessorManager
    {
        private readonly IConfiguration _configuration;
        
        public TaskProcessorManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task RunTask(APITask task)
        {
            await task.Run();
            await task.End();
        }
    }
}
