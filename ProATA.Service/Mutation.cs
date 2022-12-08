using ProATA.Service.Models;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;

namespace ProATA.Service
{
    public class Mutation
    {
        public SchedulerAddedPayload AddScheduler(string hostName, bool defaultHostname, [Service] ISchedulerRepository rep)
        {
            var scheduler = new Scheduler(Guid.NewGuid(), hostName, defaultHostname);
            rep.AddScheduler(scheduler);



            return new SchedulerAddedPayload
            {
                Scheduler = new SchedulerDto
                {
                    Id = scheduler.Id,
                    HostName = hostName,
                    DefaultHost = defaultHostname,
                },
            };
        }

        public TaskAddedPayload AddTask(string title, bool enabled, Guid schedulerId, [Service] ITaskRepository taskRep, [Service] ISchedulerRepository schedulerRep)
        {
            var scheduler = schedulerRep.GetById(schedulerId);
            var task = new APITask(Guid.NewGuid(), title, enabled, scheduler);
            taskRep.AddTask(task);

            return new TaskAddedPayload
            {
                ApiTask = new APITaskDto
                {
                    Id = task.Id,
                    Title = title,
                    Enabled = enabled,
                    Scheduler = new SchedulerDto
                    {
                        Id = scheduler.Id,
                        HostName = scheduler.HostName,
                        DefaultHost = scheduler.DefaultHost,
                    }
                },
            };
        }
    }
}
