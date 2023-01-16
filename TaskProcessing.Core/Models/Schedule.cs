using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public class Schedule : Entity<Guid>
    {
        public string CronExpression { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the schedule is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the task to be scheduled
        /// </summary>
        public APITask Task { get; set; }
    }

}
