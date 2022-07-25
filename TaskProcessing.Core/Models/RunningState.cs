namespace TaskProcessing.Core.Models
{
    internal class RunningState : TaskState
    {
        public RunningState(APITask task) : base(task)
        {
        }

        public override Task Disable()
        {
            Console.WriteLine("Cannot disable task in running state");

            return Task.CompletedTask;
        }

        public override Task Enable()
        {
            Console.WriteLine("Cannot enable task in running state");

            return Task.CompletedTask;
        }

        public override async Task End()
        {
            await _task._End();
            _task._currentState = new ReadyState(_task);
        }

        public override Task Run()
        {
            Console.WriteLine("Task is already in running state");

            return Task.CompletedTask;
        }
    }
}
