using Microsoft.Extensions.Configuration;
using NHibernate;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace TaskProcessing.Data.Repositories
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlUserRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("db1");
        }

        public User GetUserByUsername(string username)
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var query = from u in session.Query<User>()
                            select u;

                query = query.Where(u => u.UserName == username);

                var users = query.ToList();

                if (users.Count > 0)
                {
                    return query.ToList().Last();
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<User> GetUsers()
        {
            using (ISession session = SessionFactory.GetNewSession(_connectionString))
            {
                var query = session.Query<User>()

                    .OrderBy(x => x.UserName);

                return query.ToList();
            }
        }
    }

}
