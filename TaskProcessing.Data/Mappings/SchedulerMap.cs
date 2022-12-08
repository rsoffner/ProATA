using FluentNHibernate.Mapping;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Mappings
{
    public class SchedulerMap : ClassMap<Scheduler>
    {
        public SchedulerMap()
        {
            Table("Schedulers");

            Id(x => x.Id);

            Map(x => x.HostName);
            Map(x => x.DefaultHost);

            HasMany(x => x.Tasks)
                .Table("Tasks")
                .KeyColumn("SchedulerId")
                // .Inverse()
                .Not.LazyLoad()
                .Cascade.SaveUpdate();
        }
    }
}
