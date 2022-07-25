using TaskProcessing.Core.Interfaces;

namespace TaskProcessing.Core.Strategies
{
    public class WayfairLabels : IRunStrategy
    {
        public Task Run()
        {
            Console.WriteLine("Wayfair Labels running");

            return Task.CompletedTask;
        }
    }
}
