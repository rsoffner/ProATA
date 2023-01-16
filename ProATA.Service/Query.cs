using Azure;
using Microsoft.Extensions.Options;
using ProATA.Service.Models;
using ProATA.SharedKernel;
using System.Reactive.Concurrency;
using TaskProcessing.Core.Contracts;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;

namespace ProATA.Service
{
    public class Query
    {
        public DatabaseResponse<APITaskDto> GetTasks(DatatableOptions options, [Service] ITaskRepository rep)
        {
            if (options.SchedulerId == Guid.Empty)
            {
                var response = rep.GetTasks(options.Paginate.Page, options.Paginate.Limit);
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

                return new DatabaseResponse<APITaskDto>
                {
                    Data = tasks,
                    RecordsTotal = response.RecordsTotal,
                    RecordsFiltered = response.RecordsFiltered,
                };
            }
            else
            {
                var response = rep.GetTasksByScheduler((Guid)options.SchedulerId, options.Paginate.Page, options.Paginate.Limit);
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

                return new DatabaseResponse<APITaskDto>
                {
                    Data = tasks,
                    RecordsTotal = response.RecordsTotal,
                    RecordsFiltered = response.RecordsFiltered,
                };
            }
        }

        public IEnumerable<SchedulerDto> GetSchedulers([Service] ISchedulerRepository rep)
        {
            var items = rep.GetSchedulers();
            IList<SchedulerDto> schedulers = new List<SchedulerDto>();
            foreach (var scheduler in items)
            {
                schedulers.Add(new SchedulerDto {
                    Id = scheduler.Id,
                    HostName = scheduler.HostName,
                    DefaultHost = scheduler.DefaultHost,
                });
            }

            return schedulers;
        }

        public SchedulerDto GetScheduler(Guid id, [Service] ISchedulerRepository rep)
        {
            var item = rep.GetById(id);

            return new SchedulerDto 
            { 
                Id = item.Id, 
                HostName = item.HostName,
                DefaultHost = item.DefaultHost,
            };
        }
           

        public APITaskDto GetTask(Guid id, [Service] ITaskRepository rep)
        {
            var item = rep.GetTask(id);
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

            return new APITaskDto
            {
                Title = item.Title,
                Enabled = item.Enabled,
                Scheduler = scheduler,
                Schedules = schedules
            };
        }

        // Schedule
        public DatabaseResponse<ScheduleDto> GetSchedulesByTask(DatatableOptions options, [Service] IScheduleRepository rep)
        {
            IList<ScheduleDto> schedules = new List<ScheduleDto>();

            if (options.TaskId != Guid.Empty)
            {
                var response = rep.GetSchedulesByTask((Guid)options.TaskId, options.Paginate.Page, options.Paginate.Limit);

                foreach (var item in response.Data)
                {
                    schedules.Add(new ScheduleDto
                    {
                        Id = item.Id,
                        Enabled = item.Enabled,
                       CronExpression = item.CronExpression,
                    });
                }

                return new DatabaseResponse<ScheduleDto>
                {
                    Data = schedules,
                    RecordsTotal = response.RecordsTotal,
                    RecordsFiltered = response.RecordsFiltered,
                };
            }
            else
            {
                return new DatabaseResponse<ScheduleDto>
                {
                    Data = schedules,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                };
            }
        }
            
    }
}
