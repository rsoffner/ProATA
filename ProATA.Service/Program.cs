using ProATA.Service;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();

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
