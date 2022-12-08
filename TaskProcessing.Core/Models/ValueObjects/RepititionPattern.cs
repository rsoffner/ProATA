using ProATA.SharedKernel;

namespace ProATA.Logic.Model.ValueObjects
{
    public class RepetitionPattern : ValueObject<RepetitionPattern>
    {
        /// <summary>
        /// Gets or sets how long the pattern is repeated.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the amount of time between each restart of the task.
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if a running instance of the task is stopped at the end of repetition pattern duration.
        /// </summary>
        public bool StopAtDurationEnd { get; set; }

        protected override bool EqualsCore(RepetitionPattern other)
        {
            return (Duration == other.Duration && Interval == other.Interval && StopAtDurationEnd == other.StopAtDurationEnd);
        }
    }
}
