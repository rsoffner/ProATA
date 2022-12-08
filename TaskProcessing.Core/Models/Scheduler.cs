using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public class Scheduler : Entity<Guid>
    {
        public virtual string? HostName { get; set; }
        public virtual bool DefaultHost { get; set; }

        public virtual ISet<APITask> Tasks { get; set; }

        protected Scheduler()
            : base(Guid.NewGuid()) // required for NHibernate
        {

        }

        public Scheduler(Guid id, string? hostName, bool defaultHost) : base(id)
        {
            HostName = hostName;
            DefaultHost = defaultHost;
        }
    }
}
