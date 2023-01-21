using MediatR;
using ProATA.SharedKernel;
using TaskProcessing.Data.Queries;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Core.Handlers
{
    public class GetTasksHandler : IRequestHandler<GetTasksQuery, DatabaseResponse<APITaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTasksHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<DatabaseResponse<APITaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            if (request.Options.Filters[0].Value == Guid.Empty.ToString())
            {
                var response = _taskRepository.GetTasks(request.Options.Paginate.Page, request.Options.Paginate.Limit);
                IList<APITaskDto> tasks = new List<APITaskDto>();
                foreach (var item in response.Data)
                {
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
                            CronExpression = schedule.CronExpression
                        });
                    }
                    tasks.Add(new APITaskDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Enabled = item.Enabled,
                        Scheduler = scheduler,
                        Schedules = schedules
                    });
                }

                return Task.FromResult(new DatabaseResponse<APITaskDto>
                {
                    Data = tasks,
                    RecordsTotal = response.RecordsTotal,
                    RecordsFiltered = response.RecordsFiltered,
                });
            }
            else
            {
                var response = _taskRepository.GetTasksByScheduler(new Guid(request.Options.Filters[0].Value), request.Options.Paginate.Page, request.Options.Paginate.Limit);
                IList<APITaskDto> tasks = new List<APITaskDto>();
                foreach (var item in response.Data)
                {
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
                    tasks.Add(new APITaskDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Enabled = item.Enabled,
                        Scheduler = scheduler,
                        Schedules = schedules
                    });
                }

                return Task.FromResult(new DatabaseResponse<APITaskDto>
                {
                    Data = tasks,
                    RecordsTotal = response.RecordsTotal,
                    RecordsFiltered = response.RecordsFiltered,
                });
            }
        }
    }
}
