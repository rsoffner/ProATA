using FluentNHibernate.Mapping;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Mappings
{
    public class APITaskMap : ClassMap<APITaskEntity>
    {
        public APITaskMap()
        {
            Table("Tasks");

            Id(x => x.Id);

            Map(x => x.Title);
        }
    }
}
