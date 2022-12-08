using ProATA.SharedKernel;
using TaskProcessing.Core.Events;
using TaskProcessing.Core.Interfaces;

namespace TaskProcessing.Core.Models
{
    public class APITask : Entity<Guid>
    {
        public virtual string Title { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual Scheduler Scheduler { get; set; }
        public virtual ISet<Schedule>? Schedules { get; set; }

        public virtual TaskState _currentState { get; set; }
		public virtual IRunStrategy _runStrategy { get; set; }


		public APITask() { }

        public APITask(Guid id, string title, bool enabled, Scheduler scheduler) : base(id)
        {
            Id = id;    
            Title = title;
            Enabled = enabled;
            Scheduler = scheduler;

            Schedules = new HashSet<Schedule>();

            _currentState = new ReadyState(this);
        }

        public virtual void SetRunStrategy(IRunStrategy strategy)
        {
            _runStrategy = strategy;
        }

        public virtual Task Run()
        {
            if (_runStrategy == null)
            {
                throw new InvalidOperationException("Run strategy must be set");
            }

            _currentState.Run();

            return Task.CompletedTask;
        }

        public virtual Task End()
        {
            _currentState.End();

            return Task.CompletedTask;
        }

        public virtual Task Enable()
        {
            _currentState.Enable();

            return Task.CompletedTask;
        }

        public virtual Task Disable()
        {
            _currentState.Disable();

            return Task.CompletedTask;
        }

        public virtual async Task _Run()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Running));
            await _runStrategy.Run();
         }

        public virtual Task _End()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Ready));

            return Task.CompletedTask;
        }

        public virtual Task _Enable()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Ready));

            return Task.CompletedTask;
        }

        public virtual Task _Disable()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Disabled));

            return Task.CompletedTask;
        }
    }
}
