using Microsoft.EntityFrameworkCore;
using ProATA.SharedKernel;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Models;

namespace TaskProcessing.Data.Repositories
{
    public class SqlScheduleRepository : IScheduleRepository
    {
        private readonly ProATADbContext _context;

        public SqlScheduleRepository(ProATADbContext context)
        {
            _context = context;
        }
        public DatabaseResponse<Schedule> GetSchedulesByTask(Guid taskId, int page, int pageSize)
        {
            var count = _context.Schedules.Where(x => x.Task.Id == taskId).Count();

            var data = _context.Schedules.Where(x => x.Task.Id == taskId).Include(x => x.Task);

            return new DatabaseResponse<Schedule>
            {
                Data = data.Skip((page - 1) * pageSize).ToList(),
                RecordsTotal = count,
                RecordsFiltered = count
            };
        }
    }
}
