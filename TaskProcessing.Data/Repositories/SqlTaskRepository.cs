using Microsoft.Extensions.Configuration;
using NHibernate;
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

        public async Task<APITask> GetTask(Guid id, CancellationToken cancellationToken = default)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var task = await session.GetAsync<APITaskDto>(id);

                if (task != null)
                {
                    return new APITask(task.Id, task.Title, task.Enabled);
                }
                else
                {
                    return null;
                }

                
            }
        }
    }
}
