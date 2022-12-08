using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public class User : Entity<int>
    {
        protected User() { }

        public User(int id) : base(id)
        {
        }

        public virtual string UserName { get; set; }

        public virtual string DisplayName { get; set; }

    }
}
