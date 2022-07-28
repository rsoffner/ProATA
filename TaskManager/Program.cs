using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Negotiate;
using ProATA.SharedKernel;
using ProATA.SharedKernel.Interfaces;
using ProATA.SharedKernel.SignalProcessor;
using TaskManager.Hubs;
using TaskManager.WorkerServices;
using TaskProcessing.Core.Handlers;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Core.Services.TaskProcessor;
using TaskProcessing.Data.Repositories;
using static TaskProcessing.Core.Contracts.Tasks;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseLamar((context, registry) =>
    {
        // Add services to the container.
        registry.AddControllersWithViews();

        /*
        registry.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

        registry.AddAuthorization(options =>
        {
            // By default, all incoming requests will be authorized according to the default policy.
            options.FallbackPolicy = options.DefaultPolicy;
        });
        */

        // NLog: Setup NLog for Dependency injection
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        registry.AddHostedService<MessageBrokerPubSubWorker>();
        registry.AddRazorPages();
        registry.AddSignalR(hubOptions =>
        {
            hubOptions.EnableDetailedErrors = true;
        });

        registry.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tasks", Version = "v1" });
        });

        // Dependency Injection
        registry.ForSingletonOf<SignalProcessorManager>();
        registry.ForSingletonOf<TaskProcessorManager>();
        registry.For<IHandleCommand<V1.Run>>().Use<RunTaskHandler>();
        registry.For<ITaskRepository>().Use<TaskRepository>();
        registry.Scan(_ =>
        {
            _.Assembly("ProATA.SharedKernel");
            _.Assembly("TaskProcessing.Core");
            _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
        });


    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<MessageBrokerHub>("/messagebroker");
    });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasks v1")
        );

    app.Run();

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