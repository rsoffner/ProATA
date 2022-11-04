using TaskProcessing.Core.Interfaces;

namespace TaskProcessing.Core.Strategies
{
    public class NewTask : IRunStrategy
    {
        public Task Run()
        {
            Console.WriteLine("Task New Task is running");

            return Task.CompletedTask;
        }
    }
}
