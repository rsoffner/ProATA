using JasperFx.Core;
using MediatR;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;
using TaskProcessing.Data.Queries;

namespace TaskProcessing.Data.Handlers
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, APITaskDto>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<APITaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var item = _taskRepository.GetTask(request.Id);
            var scheduler = new SchedulerDto
            {
                Id = item.Scheduler.Id,
                HostName = item.Scheduler.HostName,
                DefaultHost = item.Scheduler.DefaultHost,
            };
            IList<ScheduleDto> schedules = new List<ScheduleDto>();
            foreach (var schedule in item.Schedules)
            {
                schedules.Add(new ScheduleDto
                {
                    Id = schedule.Id,
                    Enabled = schedule.Enabled,
                    CronExpression = schedule.CronExpression,
                });
            }


            return Task.FromResult(new APITaskDto
            {
                Id = scheduler.Id,
                Title = item.Title,
                Enabled = item.Enabled,
                Scheduler = scheduler,
                Schedules = schedules
            });
        }
    }
}
