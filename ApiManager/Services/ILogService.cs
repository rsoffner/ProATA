using ApiManager.Models.Api;
using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.Models;

namespace ApiManager.Services
{
    public interface ILogService
    {
        LogItem AddLogItem(LogMessage message);
        LogItem GetLogItem(int id);
        ResponseDto<LogItem> ListByPeriod(int minutes, int length, string order, string search, int userId, Guid taskId);
    }
}
