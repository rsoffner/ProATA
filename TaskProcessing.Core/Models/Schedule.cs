using ProATA.Logic.Model.ValueObjects;
using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public abstract class Schedule : Entity<Guid>
    {
        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the schedule is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the trigger is activated.
        /// </summary>
        public DateTime StartBoundery { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the trigger is deactivated. The trigger cannot start the task after it is deactivated.
        /// </summary>
        public DateTime EndBoundery { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of time that the task launched by this trigger is allowed to run. 
        /// </summary>
        public TimeSpan ExecutionTimeLimit { get; set; }

        public bool Repeat { get; set; }

        /// <summary>
        /// Gets a RepetitionPattern instance that indicates how often the task is run and how long the repetition pattern is repeated after the task is started.
        /// </summary>
        public RepetitionPattern Repetition { get; set; }

        /// <summary>
        /// Gets or sets the task to be scheduled
        /// </summary>
        public APITask Task { get; set; }
    }

}
