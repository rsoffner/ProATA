using FluentNHibernate.Mapping;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Mappings
{
    public class APITaskMap : ClassMap<APITaskDto>
    {
        public APITaskMap()
        {
            Table("Tasks");

            Id(x => x.Id);

            Map(x => x.Title);
        }
    }
}
