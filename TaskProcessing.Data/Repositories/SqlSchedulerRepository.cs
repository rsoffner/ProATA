using Microsoft.Extensions.Configuration;
using NHibernate;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace TaskProcessing.Data.Repositories
{
    public class SqlSchedulerRepository : ISchedulerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlSchedulerRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("db1");
        }

        public void AddScheduler(Scheduler scheduler)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(scheduler);
                    transaction.Commit();
                }
            }
        }

        public Scheduler GetById(Guid schedulerId)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var scheduler =session.Get<Scheduler>(schedulerId);

                return scheduler;
            }
        }

        public IEnumerable<Scheduler> GetSchedulers()
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var query = session.Query<Scheduler>()
                    .OrderBy(x => x.HostName);

                return query.ToList();
            }
        }
    }

}
