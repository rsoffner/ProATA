using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Models
{
    internal class ScheduleEntityTypeConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> scheduleConfiguration) 
        {
            scheduleConfiguration.ToTable("Schedules");
            scheduleConfiguration
                .HasDiscriminator<string>("Type")
                .HasValue<Schedule>("Base")
                .HasValue<TimeSchedule>("Time")
                .HasValue<DailySchedule>("Daily")
                .HasValue<WeeklySchedule>("Weekly");

            scheduleConfiguration.OwnsOne(x => x.Repetition)
                .Property(x => x.Interval).HasColumnName("RepetitionInterval");
            scheduleConfiguration.OwnsOne(x => x.Repetition)
                .Property(x => x.Duration).HasColumnName("RepetitionDuration");
            scheduleConfiguration.OwnsOne(x => x.Repetition)
                .Property(x => x.StopAtDurationEnd).HasColumnName("RepetitionStopAtDurationEnd");

            scheduleConfiguration.Ignore(x => x.Events);
        }
    }
}
