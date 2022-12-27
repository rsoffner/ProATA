using Microsoft.EntityFrameworkCore;
using ProATA.Service;
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

builder.Services.AddCors();

builder.Services.AddScoped<ITaskRepository, SqlTaskRepository>();
builder.Services.AddScoped<ISchedulerRepository, SqlSchedulerRepository>();

builder.Services.AddGraphQLServer().AddQueryType<Query>().AddMutationType<Mutation>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.MapGraphQL();

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

app.Run();
