using Microsoft.EntityFrameworkCore;
using ProATA.SharedKernel;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Models;

namespace TaskProcessing.Data.Repositories
{
    public class SqlTaskRepository : ITaskRepository
    {
        private readonly ProATADbContext _context;

        public SqlTaskRepository(ProATADbContext context)
        {
           _context = context;
        }

        public APITask GetTask(Guid id)
        {
            var task = _context.Tasks.Include(x => x.Scheduler).Include(x => x.Schedules).Single(x => x.Id == id);

            return task;

        }

        public IEnumerable<APITask> GetAllTasks()
        {
            var tasks = _context.Tasks.OrderBy(x => x.Title);

            return tasks.ToList();
        }

        public DatabaseResponse<APITask> GetTasks(int page, int pageSize)
        {
            var count = _context.Tasks.Count();

            var data = _context.Tasks.Include(x => x.Scheduler).Include(x => x.Schedules);

            return new DatabaseResponse<APITask>()
            {
                Data = data.Take(pageSize).Skip((page - 1) * pageSize).ToList(),
                RecordsTotal = count,
                RecordsFiltered = count
            };
        }

        public DatabaseResponse<APITask> GetTasksByScheduler(Guid schedulerId, int page, int pageSize)
        {
            var data = _context.Tasks.Include(x => x.Scheduler).Include(x => x.Schedules).Where(x => x.Scheduler.Id == schedulerId);

            var recordsTotal = _context.Tasks.Count();
            var recordsFiltered = data.Count();

            return new DatabaseResponse<APITask>()
            {
                Data = data.Take(pageSize).Skip((page - 1) * pageSize).ToList(),
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered
            };
        }

        public void AddTask(APITask task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();

        }

        public void DeleteTask(APITask task)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }
    }
}
