using ApiManager.Models.Api;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.MessageBrokers.Models;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;

        public LogService(ILogRepository logRepository, IUserRepository userRepository)
        {
            _logRepository = logRepository;
            _userRepository = userRepository;
        }

        public LogItem AddLogItem(LogMessage message)
        {
            int userId = 1;

            User user = _userRepository.GetUserByUsername(message.UserName);

            try
            {
                LogItem logItem = new LogItem(message.TimeStamp,
                    (int)message.ErrorType,
                    message.Message,
                    Enum.GetName(typeof(ErrorType), (int)message.ErrorType),
                    message.Url,
                    message.Detail,
                    false,
                    user.Id,
                    0,
                    message.Source);

                _logRepository.AddLogItem(logItem);

                return logItem;
            }
            catch
            {
                return null;
            }
        }

        public LogItem GetLogItem(int id)
        {
            return _logRepository.GetLogItem(id);
        }

        public ResponseDto<LogItem> ListByPeriod(int minutes, int length, string order, string search, int userId, Guid taskId)
        {
            var items = _logRepository.ListByPeriod(minutes, order, search, userId, taskId);

            return new ResponseDto<LogItem>
            {
                Draw = length,
                Data = (IList<LogItem>)items,
                RecordsTotal = items.Count(),
                RecordsFiltered = items.Count()
            };
        }
    }

}
