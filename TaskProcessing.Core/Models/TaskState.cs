namespace TaskProcessing.Core.Models
{
    public abstract class TaskState
    {
        protected readonly APITask _task;

        public TaskState(APITask task)
        {
            _task = task;
        }

        public abstract Task Run();
        public abstract Task End();
        public abstract Task Enable();
        public abstract Task Disable();
    }
}
