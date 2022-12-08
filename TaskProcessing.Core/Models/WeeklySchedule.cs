using ProATA.SharedKernel.Enums;

namespace TaskProcessing.Core.Models
{
    public class WeeklySchedule : Schedule
    {
        /// <summary>
        /// Gets or sets the days of the week on which the task runs.
        /// </summary>
        public virtual DaysOfTheWeek DaysOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the interval between the weeks in the schedule.
        /// </summary>
        public virtual short WeeksInterval { get; set; }
    }
}
