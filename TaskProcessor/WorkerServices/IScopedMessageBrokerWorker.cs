namespace TaskProcessor.WorkerServices
{
    public interface IScopedMessageBrokerWorker
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}