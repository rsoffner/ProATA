using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public class Scheduler : Entity<Guid>
    {
        public string? HostName { get; set; }
        public bool DefaultHost { get; set; }

        public IList<APITask> Tasks { get; set; }

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
