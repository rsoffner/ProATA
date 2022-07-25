using ProATA.SharedKernel.Interfaces;

namespace TaskProcessing.Core.Contracts
{
    public static class Tasks
    {
        public static class V1
        {
            public class Create
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
            }

            public class Run
            {
                public Guid Id { get; set; }
            }

            public class End
            {
                public Guid Id { get; set; }
            }


            public class Enable
            {
                public Guid Id { get; set; }
            }

            public class Disable
            {
                public Guid Id { get; set; }
            }
        }
    }
}
