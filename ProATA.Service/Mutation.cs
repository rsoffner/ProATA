using ProATA.Service.Models;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

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
                Scheduler = scheduler,
            };
        }

        public TaskAddedPayload AddTask(string title, bool enabled, Guid schedulerId, [Service] ITaskRepository taskRep, [Service] ISchedulerRepository schedulerRep)
        {
            var scheduler = schedulerRep.GetById(schedulerId);
            var task = new APITask(Guid.NewGuid(), title, enabled, scheduler);
            taskRep.AddTask(task);

            return new TaskAddedPayload
            {
                ApiTask = task,
            };
        }
    }
}
