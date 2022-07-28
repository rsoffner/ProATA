using Microsoft.Extensions.Configuration;
using NHibernate;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TaskRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("db1");
        }

        public APITask GetTask(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var task = session.Get<APITaskEntity>(id);

                return new APITask(task.Id, task.Title);
            }
        }
    }
}
