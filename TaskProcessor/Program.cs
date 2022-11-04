using Lamar.Microsoft.DependencyInjection;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using NLog;
using NLog.Web;
using ProATA.SharedKernel.Interfaces;
using TaskProcessing.Core.Handlers;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;
using TaskProcessing.Data.Repositories;
using TaskProcessor.WorkerServices;
using static TaskProcessing.Core.Contracts.Tasks;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .UseLamar((context, registry) =>
        {
            registry.AddHostedService<MessageBrokerWorker>();

            registry.For<IScopedMessageBrokerWorker>().Use<ScopedMessageBrokerWorker>();
            registry.ForSingletonOf<TaskProcessorManager>();
            registry.For<IHandleCommand<V1.Run>>().Use<RunTaskHandler>();
            registry.For<ITaskRepository>().Use<GraphQLTaskRepository>();
            registry.Scan(_ =>
            {
                _.Assembly("ProATA.SharedKernel");
                _.Assembly("TaskProcessing.Core");
                _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
            });
        })
        .UseNLog()
        .Build();

    await host.RunAsync();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
/*
IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Api Task Processor Service";
    })
    .ConfigureServices(services =>
    {
        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);

        services.AddHostedService<MessageBrokerWorker>();
        
    })
    .Build();

await host.RunAsync();
*/