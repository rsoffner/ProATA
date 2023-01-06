using Newtonsoft.Json;
using ProATA.Logic.Model.ValueObjects;

namespace TaskProcessing.Data.Entities
{
    public class ScheduleDto
    {
        public virtual Guid Id { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the schedule is enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the trigger is activated.
        /// </summary>
        public virtual DateTime StartBoundery { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the trigger is deactivated. The trigger cannot start the task after it is deactivated.
        /// </summary>
        public virtual DateTime EndBoundery { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of time that the task launched by this trigger is allowed to run. 
        /// </summary>
        public virtual TimeSpan ExecutionTimeLimit { get; set; }

        public virtual bool Repeat { get; set; }

        /// <summary>
        /// Gets a RepetitionPattern instance that indicates how often the task is run and how long the repetition pattern is repeated after the task is started.
        /// </summary>
        public virtual RepetitionPattern Repetition { get; set; }

        /// <summary>
        /// Gets or sets the task to be scheduled
        /// </summary>
        public virtual APITaskDto Task { get; set; }
    }
}
