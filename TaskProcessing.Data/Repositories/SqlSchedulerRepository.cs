using Microsoft.EntityFrameworkCore;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Models;

namespace TaskProcessing.Data.Repositories
{
    public class SqlSchedulerRepository : ISchedulerRepository
    {
        private readonly ProATADbContext _context;

        public SqlSchedulerRepository(ProATADbContext context)
        {
            _context = context;
        }

        public void AddScheduler(Scheduler scheduler)
        {
            _context.Schedulers.Add(scheduler);
            _context.SaveChanges();
        }

        public Scheduler GetById(Guid schedulerId)
        {
            var scheduler = _context.Schedulers
                .Include(x => x.Tasks).Single(x => x.Id == schedulerId);

            return scheduler;
        }

        public Scheduler GetByHostName(string hostName)
        {
            var scheduler = _context.Schedulers
                .Include(x => x.Tasks).ThenInclude(x => x.Schedules).Single(x => x.HostName == hostName);

            return scheduler;  
        }

        public IEnumerable<Scheduler> GetSchedulers()
        {
            var schedulers = _context.Schedulers.OrderBy(x => x.HostName);

            return schedulers.ToList();
        }
    }

}
