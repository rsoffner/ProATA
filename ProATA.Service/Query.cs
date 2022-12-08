using ProATA.Service.Models;
using ProATA.SharedKernel;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace ProATA.Service
{
    public class Query
    {
        public DatabaseResponse<APITask> GetTasks(DatatableOptions options, [Service] ITaskRepository rep)
        {
            if (options.SchedulerId == null)
            {
                return rep.GetTasks(options.Paginate.Page, options.Paginate.Limit);
            }
            else
            {
                return rep.GetTasksByScheduler((Guid)options.SchedulerId, options.Paginate.Page, options.Paginate.Limit);
            }
        }

        public IEnumerable<Scheduler> GetSchedulers([Service] ISchedulerRepository rep)
        {
            return rep.GetSchedulers();
        }

        public Scheduler GetScheduler(Guid id, [Service] ISchedulerRepository rep) =>
            rep.GetById(id);

        public APITask GetTask(Guid id, [Service] ITaskRepository rep) =>
            rep.GetTask(id);
    }
}
