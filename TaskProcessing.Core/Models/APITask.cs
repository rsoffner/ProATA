using ProATA.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;
using TaskProcessing.Core.Events;
using TaskProcessing.Core.Interfaces;

namespace TaskProcessing.Core.Models
{
    public class APITask : Entity<Guid>
    {
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public Scheduler Scheduler { get; set; }
        public IList<Schedule> Schedules { get; set; }

        [NotMapped]
        public TaskState CurrentState { get; set; }

        [NotMapped]
		public IRunStrategy RunStrategy { get; set; }


		public APITask() { }

        public APITask(Guid id, string title, bool enabled, Scheduler scheduler) : base(id)
        {
            Id = id;    
            Title = title;
            Enabled = enabled;
            Scheduler = scheduler;

            Schedules = new List<Schedule>();

            CurrentState = new ReadyState(this);
        }

        public Task Run()
        {
            if (RunStrategy == null)
            {
                throw new InvalidOperationException("Run strategy must be set");
            }

            CurrentState.Run();

            return Task.CompletedTask;
        }

        public Task End()
        {
            CurrentState.End();

            return Task.CompletedTask;
        }

        public Task Enable()
        {
            CurrentState.Enable();

            return Task.CompletedTask;
        }

        public Task Disable()
        {
            CurrentState.Disable();

            return Task.CompletedTask;
        }

        public async Task _Run()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Running));
            await RunStrategy.Run();
         }

        public Task _End()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Ready));

            return Task.CompletedTask;
        }

        public Task _Enable()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Ready));

            return Task.CompletedTask;
        }

        public Task _Disable()
        {
            Events.Add(new TaskStateChangedEvent(this.Id, ProATA.SharedKernel.Enums.TaskState.Disabled));

            return Task.CompletedTask;
        }
    }
}
