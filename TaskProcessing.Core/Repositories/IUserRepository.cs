using TaskProcessing.Core.Models;

namespace TaskProcessing.Core.Repositories
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);

        IEnumerable<User> GetUsers();
    }
}
