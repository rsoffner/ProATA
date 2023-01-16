using Newtonsoft.Json;
using ProATA.Logic.Model.ValueObjects;

namespace TaskProcessing.Data.Entities
{
    public class ScheduleDto
    {
        public virtual Guid Id { get; set; }

        public virtual string CronExpression { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the schedule is enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the task to be scheduled
        /// </summary>
        public virtual APITaskDto Task { get; set; }
    }
}
