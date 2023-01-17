using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProATA.SharedKernel.Interfaces;
using Quartz;
using System.Globalization;
using System.Text.RegularExpressions;
using TaskProcessing.Core.Interfaces;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskScheduler;

namespace TaskProcessor.Services.TaskScheduler
{
    public class TaskJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            
            return Task.CompletedTask;
        }
    }

    public class TaskSchedulerManager : ITaskSchedulerManager
    {
        private readonly ISchedulerRepository _schedulerRepository;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IConfiguration _configuration;
        private IScheduler _scheduler;
        private readonly ILogger<TaskSchedulerManager> _logger;
        private readonly IList<APITask> _tasks;

        public TaskSchedulerManager(ISchedulerRepository schedulerRepository, IConfiguration configuration, ISchedulerFactory schedulerFactory, ILogger<TaskSchedulerManager> logger)
        {
            _schedulerRepository = schedulerRepository;
            _schedulerFactory = schedulerFactory;
            _configuration = configuration;

            _tasks = new List<APITask>();
            _logger = logger;
        }

        public APITask GetTask(Guid taskId)
        {
            return _tasks.Single(x => x.Id == taskId);
        }

        public async Task StartScheduler()
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

                // Get schedules and add it to the scheduler
                _scheduler = await _schedulerFactory.GetScheduler();

                var job = JobBuilder.Create<TaskJob>()
                    .WithIdentity(task.Id.ToString(), "APIScheduler")
                    .Build();

                IList<ITrigger> triggers = new List<ITrigger>();
                foreach (var schedule in task.Schedules)
                {

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity(schedule.Id.ToString(), task.Id.ToString())
                        .WithCronSchedule(schedule.CronExpression)
                        .Build();

                    triggers.Add(trigger);
                }

                await _scheduler.ScheduleJob(job, (IReadOnlyCollection<ITrigger>)triggers, replace: true);
            }
        }
    }
}
