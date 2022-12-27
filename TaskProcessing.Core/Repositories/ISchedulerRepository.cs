using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface ISchedulerRepository
    {
        Scheduler GetById(Guid schedulerId);
        IEnumerable<Scheduler> GetSchedulers();
        void AddScheduler(Scheduler scheduler);
        Scheduler GetByHostName(string hostName);
    }
}
