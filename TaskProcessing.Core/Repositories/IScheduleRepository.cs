using ProATA.SharedKernel;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface IScheduleRepository
    {
        void AddSchedule(Schedule schedule);
        DatabaseResponse<Schedule> GetSchedulesByTask(Guid taskId, int page, int pageSize);
    }
}
