namespace TaskProcessing.Core.Models
{
    internal class DisabledState : TaskState
    {
        public DisabledState(APITask task) : base(task)
        {
        }

        public override Task Disable()
        {
            Console.WriteLine("Task is already in disabled state");

            return Task.CompletedTask;
        }

        public override async Task Enable()
        {
            await _task._Enable();
            _task._currentState = new ReadyState(_task);
        }

        public override Task End()
        {
            Console.WriteLine("Cannot end task in disabled state");

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Console.WriteLine("Cannot run task in disabled state");

            return Task.CompletedTask;
        }
    }
}
