using FluentNHibernate.Mapping;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Mappings
{
    public class APITaskMap : ClassMap<APITask>
    {
        public APITaskMap()
        {
            Table("Tasks");

            Id(x => x.Id).GeneratedBy.GuidComb();

            Map(x => x.Title);
            Map(x => x.Enabled);

            HasMany(x => x.Schedules)
                .Table("Schedules")
                .KeyColumn("TaskId")
                // .Inverse()
                .Not.LazyLoad()
                .Cascade.SaveUpdate();

            References(x => x.Scheduler).Column("SchedulerId");
        }
    }
}
