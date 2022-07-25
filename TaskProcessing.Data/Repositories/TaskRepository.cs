using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace TaskProcessing.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public APITask GetTask(Guid id)
        {
            var task = new APITask(id, "Test");

            return task;
        }
    }
}
