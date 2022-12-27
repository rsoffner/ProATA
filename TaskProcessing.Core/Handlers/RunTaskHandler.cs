using ProATA.SharedKernel.Interfaces;
using TaskProcessing.Core.Contracts;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;

namespace TaskProcessing.Core.Handlers
{
    public class RunTaskHandler : IHandleCommand<Tasks.V1.Run>
    {
        private readonly ITaskProcessorManager _taskProcessorManager;
        private readonly ITaskRepository _taskRepository;

        public RunTaskHandler(ITaskProcessorManager taskProcessorManager, ITaskRepository taskRepository)
        {
            _taskProcessorManager = taskProcessorManager;
            _taskRepository = taskRepository;
        }
        
        public async Task Handle(Tasks.V1.Run command)
        {
            APITask task =  _taskRepository.GetTask(command.Id);

            await _taskProcessorManager.RunTask(task);
        }
    }
}
