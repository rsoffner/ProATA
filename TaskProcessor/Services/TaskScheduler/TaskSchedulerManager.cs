using ProATA.SharedKernel.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;
using TaskProcessing.Core.Interfaces;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskScheduler;

namespace TaskProcessor.Services.TaskScheduler
{
    public class TaskSchedulerManager : ITaskSchedulerManager
    {
        private readonly ISchedulerRepository _schedulerRepository;
        private readonly IConfiguration _configuration;
        private readonly IList<APITask> _tasks;

        public TaskSchedulerManager(ISchedulerRepository schedulerRepository, IConfiguration configuration)
        {
            _schedulerRepository = schedulerRepository;
            _configuration = configuration;

            _tasks = new List<APITask>();
        }

        public APITask GetTask(Guid taskId)
        {
            return _tasks.Single(x => x.Id == taskId);
        }

        public void StartScheduler()
        {
            var scheduler = _schedulerRepository.GetByHostName(_configuration["Scheduler:HostName"]);

            foreach (var task in scheduler.Tasks)
            {
                TextInfo info = new CultureInfo("en-US", false).TextInfo;
                string baseClassname = Regex.Replace(info.ToTitleCase(task.Title), @"\s+", "");

                string strategyName = "TaskProcessing.Core.Strategies." + baseClassname;
                Type strategyType = Type.GetType(strategyName + ", TaskProcessing.Core");
                if (strategyType != null)
                {
                    var strategy = Activator.CreateInstance(strategyType) as IRunStrategy;
                    if (strategy != null)
                    {
                        task.RunStrategy = strategy;
                    }
                }
                task.CurrentState = new ReadyState(task);
                task.Events = new List<IDomainEvent>();

                _tasks.Add(task);
            }
        }
    }
}
