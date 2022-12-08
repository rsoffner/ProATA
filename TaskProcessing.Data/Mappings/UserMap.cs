using FluentNHibernate.Mapping;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("EEK_API_USERS");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.UserName);
            Map(x => x.DisplayName);
        }
    }
}
