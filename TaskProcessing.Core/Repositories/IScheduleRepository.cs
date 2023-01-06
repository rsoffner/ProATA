using ProATA.SharedKernel;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface IScheduleRepository
    {
        DatabaseResponse<Schedule> GetSchedulesByTask(Guid taskId, int page, int pageSize);
    }
}
