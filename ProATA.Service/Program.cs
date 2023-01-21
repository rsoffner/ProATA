using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data;
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
builder.Services.AddScoped<IScheduleRepository, SqlScheduleRepository>();
builder.Services.AddMediatR(typeof(TaskProcessingDataMediatREntrypoint).Assembly);


//builder.Services.AddGraphQLServer().AddQueryType<Query>().AddMutationType<Mutation>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

// app.MapGraphQL();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
