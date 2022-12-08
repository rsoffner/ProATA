using FluentNHibernate.Mapping;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Mappings
{
    public class LogItemMap : ClassMap<LogItem>
    {
        public LogItemMap()
        {
            Table("EEK_API_LOG");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.TimeStamp);
            Map(x => x.Priority).Column("type");
            Map(x => x.Message).Column("event");
            Map(x => x.PriorityName).Column("error_type");
            Map(x => x.Url).Column("url");
            Map(x => x.Detail).Column("trace");
            Map(x => x.Acknowledged).Column("acknowledged");
            Map(x => x.Duration).Column("duration").Nullable();
            Map(x => x.Source).Column("source").Nullable();
            Map(x => x.UserId).Column("userId");
            Map(x => x.TaskId).Column("taskId");

            // References(x => x.User);
        }
    }
}
