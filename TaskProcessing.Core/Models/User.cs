using ProATA.SharedKernel;

namespace TaskProcessing.Core.Models
{
    public class User : Entity<int>
    {
        protected User() { }

        public User(int id) : base(id)
        {
        }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

    }
}
