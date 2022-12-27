﻿namespace TaskProcessing.Core.Models
{
    public class ReadyState : TaskState
    {
        public ReadyState(APITask task) : base(task)
        {
        }

        public override async Task Disable()
        {
            await _task.Disable();
            _task.CurrentState = new DisabledState(_task);
        }

        public override Task Enable()
        {
            Console.WriteLine("Task is already in ready state");

            return Task.CompletedTask;
        }

        public override Task End()
        {
            Console.WriteLine("Task is already in ready state");

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            await _task._Run();
            _task.CurrentState = new RunningState(_task);
        }
    }
}
