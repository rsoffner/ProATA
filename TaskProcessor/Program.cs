using askProcessing.Core.Services.SignalProcessor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;
using TaskProcesser.Services.TaskProcessor;
using TaskProcessing.Core.MessageBrokers.Subscribers;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;
using TaskProcessing.Core.Services.TaskScheduler;
using TaskProcessing.Data;
using TaskProcessing.Data.Models;
using TaskProcessing.Data.Repositories;
using TaskProcessor;
using TaskProcessor.Services.SignalProcessor;
using TaskProcessor.Services.TaskScheduler;
using TaskProcessor.WorkerServices;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

var connectionString = config.GetConnectionString("db1");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging();
        services.AddHostedService<MessageBrokerWorker>();

        services.AddDbContext<ProATADbContext>
            (options => options.UseSqlServer(connectionString));

        services.AddScoped<IScopedMessageBrokerWorker, ScopedMessageBrokerWorker>();
        services.AddScoped<SubscriberBase, SubscriberRabbitMq>();
        services.AddScoped<ITaskProcessorManager, TaskProcessorManager>();
        services.AddScoped<ITaskRepository, SqlTaskRepository>();
        services.AddScoped<ISchedulerRepository, SqlSchedulerRepository>();
        services.AddScoped<IScheduleRepository, SqlScheduleRepository>();
        services.AddScoped<ITaskSchedulerManager, TaskSchedulerManager>();
        services.AddScoped<ISignalProcessorManager, SignalProcessorManager>();
        services.AddMediatR(typeof(TaskProcessingDataMediatREntrypoint).Assembly, typeof(TaskProcessorMediatREntrypoint).Assembly);

        services.AddQuartz(q =>
        {
            // base Quartz scheduler, job and trigger configuration
        });

        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
    }).Build();

await host.RunAsync();
