using ApiManager.Hubs;
using ApiManager.Services;
using ApiManager.Services.SignalProcessor;
using askProcessing.Core.Services.SignalProcessor;
using Microsoft.EntityFrameworkCore;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Models;
using TaskProcessing.Data.Repositories;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

var connectionString = config.GetConnectionString("db1");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProATADbContext>
            (options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILogRepository, SqlLogRepository>();
builder.Services.AddScoped<ITaskRepository, SqlTaskRepository>();
builder.Services.AddScoped<ISchedulerRepository, SqlSchedulerRepository>();
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ISignalProcessorManager, SignalProcessorManager>();

builder.Services.AddSignalR();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<MessageBrokerHub>("/messagebroker");

app.Run();
