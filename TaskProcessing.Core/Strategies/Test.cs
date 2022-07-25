using TaskProcessing.Core.Interfaces;

namespace TaskProcessing.Core.Strategies
{
    public class Test : IRunStrategy
    {
        public Task Run()
        {
            Console.WriteLine("Task Test is running");

            return Task.CompletedTask;
        }
    }
}
