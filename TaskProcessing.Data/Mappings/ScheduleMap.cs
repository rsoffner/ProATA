using FluentNHibernate.Mapping;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Mappings
{
    public class ScheduleMap : ClassMap<Schedule>
    {
        public ScheduleMap()
        {
            Table("Schedules");

            Id(x => x.Id).GeneratedBy.GuidComb();

            DiscriminateSubClassesOnColumn("Type").Not.Nullable();

            Map(x => x.StartBoundery);
            Map(x => x.EndBoundery);
            Map(x => x.ExecutionTimeLimit);
            Map(x => x.Repeat);
            Component(x => x.Repetition, m =>
            {
                m.Map(x => x.Interval).Column("RepetitionInterval");
                m.Map(x => x.Duration).Column("RepetitionDuration");
                m.Map(x => x.StopAtDurationEnd).Column("RepetitionStopAtDurationEnd");
            });
            Map(x => x.Enabled);
            // Map(x => x.ScheduleType).Column("Type").CustomType(typeof(ScheduleType));

            References(x => x.Task).Column("TaskId");
        }
    }
    public class TimeScheduleMap : SubclassMap<TimeSchedule>
    {
        public TimeScheduleMap()
        {
            DiscriminatorValue("Time");
        }
    }

    public class DailyScheduleMap : SubclassMap<DailySchedule>
    {
        public DailyScheduleMap()
        {
            DiscriminatorValue("Daily");

            Map(x => x.DaysInterval);
        }
    }

    public class WeeklyScheduleMap : SubclassMap<WeeklySchedule>
    {
        public WeeklyScheduleMap()
        {
            DiscriminatorValue("Weekly");

            Map(x => x.DaysOfWeek).CustomType(typeof(DaysOfTheWeek));
            Map(x => x.WeeksInterval);
        }
    }
}
