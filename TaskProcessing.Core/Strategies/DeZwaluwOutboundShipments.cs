using TaskProcessing.Core.Interfaces;

namespace TaskProcessing.Core.Strategies
{
    public class DeZwaluwOutboundShipments : IRunStrategy
    {
        public Task Run()
        {
            Console.WriteLine("De Zwaluw Outbound Shipments running");

            return Task.CompletedTask;
        }
    }
}
