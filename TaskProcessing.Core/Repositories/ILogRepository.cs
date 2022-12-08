using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface ILogRepository
    {
        void AddLogItem(LogItem logItem);
        LogItem GetLogItem(int id);
        IEnumerable<LogItem> ListByPeriod(int minutes, string order, string search, int userId, Guid taskId);
    }
}
