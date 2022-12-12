using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using ProATA.SharedKernel;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.GraphQL;

namespace TaskProcessing.Data.Repositories
{
    public class GraphQLTaskRepository : ITaskRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;

        public GraphQLTaskRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration["AppSettings:EndpointUrl"];
        }

        public void AddTask(APITask task)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<APITask> GetAllTasks()
		{
			throw new NotImplementedException();
		}

		public APITask GetTask(Guid id)
        {
            using (GraphQLHttpClient _client = new GraphQLHttpClient(_url, new NewtonsoftJsonSerializer()))
            {
                var request = new GraphQLHttpRequest
                {
                    Query = @"
                        query Task($id: UUID!) {
                            task(id: $id) {
                                title
                                enabled
                                scheduler {
                                    id
                                    hostName
                                    defaultHost
                                }
                            }
                        }
                   ",
                    OperationName = "Task",
                    Variables = new
                    {
                        id = id,
                    }
                };

                var response = _client.SendQueryAsync<TaskResponse>(request);


                if (response.Result.Errors == null)
                {
                    var scheduler = new Scheduler(response.Result.Data.Task.Scheduler.Id, response.Result.Data.Task.Scheduler.HostName, response.Result.Data.Task.Scheduler.DefaultHost);
                    var task = new APITask(id, response.Result.Data.Task.Title, response.Result.Data.Task.Enabled, scheduler);

                    return task;
                }
                else
                {
                    return null;
                }
                
            }
        }

        public DatabaseResponse<APITask> GetTasks(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public DatabaseResponse<APITask> GetTasksByScheduler(Guid schedulerId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
