using MediatR;
using TaskProcesser.Commands;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Models.ValueObjects;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;

namespace TaskProcesser.Handlers
{
    public class RunTaskHandler : IRequestHandler<RunTaskCommand, TaskResponse>
    {
        private readonly ITaskProcessorManager _taskProcessorManager;
        private readonly ITaskRepository _taskRepository;

        public RunTaskHandler(ITaskProcessorManager taskProcessorManager, ITaskRepository taskRepository)
        {
            _taskProcessorManager = taskProcessorManager;
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponse> Handle(RunTaskCommand request, CancellationToken cancellationToken)
        {
            APITask task = _taskRepository.GetTask(request.TaskId);

            return await _taskProcessorManager.RunTask(task);
        }
    }
}
