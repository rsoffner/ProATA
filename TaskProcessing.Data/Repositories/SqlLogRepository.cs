using Microsoft.Extensions.Configuration;
using NHibernate;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace TaskProcessing.Data.Repositories
{
    public class SqlLogRepository : ILogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlLogRepository(IConfiguration configuration) 
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("db1");
        }
        public void AddLogItem(LogItem logItem)
        {
            using ISession session = SessionFactory.GetNewSession(_connectionString);
            using ITransaction transaction = session.BeginTransaction();

            session.Save(logItem);
            transaction.Commit();
        }

        public LogItem GetLogItem(int id)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var sql = "exec readone_EEK_API_LOG :id";

                var result = session.CreateSQLQuery(sql)
                    .AddEntity(typeof(LogItem))
                    .SetParameter("id", id);

                var items = result.List<LogItem>();

                if (items.Count() > 0)
                {
                    return items.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<LogItem> ListByPeriod(int minutes, string order, string search, int userId, Guid taskId)
        {
            DateTime now = DateTime.Now.AddMinutes(-1);
            DateTime before = now.AddMinutes(-1 * minutes);

            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var command = "exec read_EEK_API_LOG :datestart, :dateend";

                var result = session.CreateSQLQuery(command)
                    .AddEntity(typeof(LogItem))
                    .SetParameter("datestart", before, NHibernateUtil.DateTime)
                    .SetParameter("dateend", now, NHibernateUtil.DateTime);

                var items = result.List<LogItem>();

                var sort = order.Split('.');
                if (sort[1] == "desc")
                {
                    switch (sort[0])
                    {
                        case "priorityName":
                            items = items.OrderByDescending(x => x.PriorityName).ToList();
                            break;
                        case "source":
                            items = items.OrderByDescending(x => x.Source).ToList();
                            break;
                        case "timeStamp":
                        default:
                            items = items.OrderByDescending(x => x.TimeStamp).ToList();
                            break;
                    }
                }
                else
                {
                    switch (sort[0])
                    {
                        case "priorityName":
                            items = items.OrderBy(x => x.PriorityName).ToList();
                            break;
                        case "source":
                            items = items.OrderBy(x => x.Source).ToList();
                            break;
                        case "timeStamp":
                        default:
                            items = items.OrderBy(x => x.TimeStamp).ToList();
                            break;
                    }
                }

                if (!String.IsNullOrEmpty(search))
                {
                    items = items.Where(x => x.Message.Contains(search)).ToList();
                }

                if (userId > 0)
                {
                    items = items.Where(x => x.UserId == userId).ToList();
                }

                if (taskId != Guid.Empty)
                {
                    items = items.Where(x => x.TaskId == taskId).ToList();
                }

                return items;
            }
        }
    }
}
