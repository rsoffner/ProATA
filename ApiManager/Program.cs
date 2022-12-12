using ApiManager.Hubs;
using ApiManager.Services;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILogRepository, SqlLogRepository>();
builder.Services.AddScoped<ITaskRepository, GraphQLTaskRepository>();
builder.Services.AddScoped<ISchedulerRepository, GraphQLSchedulerRepository>();
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<ILogService, LogService>();

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
