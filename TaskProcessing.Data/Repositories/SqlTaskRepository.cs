using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using ProATA.SharedKernel;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Repositories
{
    public class SqlTaskRepository : ITaskRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlTaskRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("db1");
        }

        public APITask GetTask(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var task = session.Get<APITaskDto>(id);

                if (task != null)
                {
                    var scheduler = new Scheduler(task.Scheduler.Id, task.Scheduler.HostName, task.Scheduler.DefaultHost);
                    return new APITask(task.Id, task.Title, task.Enabled, scheduler);
                }
                else
                {
                    return null;
                }

                
            }
        }

        public IEnumerable<APITask> GetAllTasks()
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var query = session.Query<APITask>()

                    .OrderBy(x => x.Title);

                return query.ToList();
            }
        }

        public DatabaseResponse<APITask> GetTasks(int page, int pageSize)
        {
            using ISession session = SessionFactory.GetNewSession(_connectionString);
            var count = session.CreateCriteria<APITask>()
                    .SetProjection(Projections.Count(Projections.Id()))
                    .FutureValue<int>();

            var query = session.Query<APITask>()
                .OrderBy(x => x.Title)
                .FetchMany(x => x.Schedules)
                .Fetch(x => x.Scheduler)
                .Take(pageSize)
                .Skip((page - 1) * pageSize);

            return new DatabaseResponse<APITask>()
            {
                Data = query.ToList(),
                RecordsTotal = count.Value,
                RecordsFiltered = count.Value
            };
        }

        public DatabaseResponse<APITask> GetTasksByScheduler(Guid schedulerId, int page, int pageSize)
        {
            using ISession session = SessionFactory.GetNewSession(_connectionString);
            var count = session.CreateCriteria<APITask>()
                    .SetProjection(Projections.Count(Projections.Id()))
                    .FutureValue<int>();

            var query = session.Query<APITask>()
                .OrderBy(x => x.Title)
                .Where(x => x.Scheduler.Id == schedulerId)
                .FetchMany(x => x.Schedules)
                .Fetch(x => x.Scheduler)
                .Take(pageSize)
                .Skip((page - 1) * pageSize);

            return new DatabaseResponse<APITask>()
            {
                Data = query.ToList(),
                RecordsTotal = count.Value,
                RecordsFiltered = count.Value
            };
        }

        public void AddTask(APITask task)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(task);
                    transaction.Commit();
                }
            }
        }
    }
}
