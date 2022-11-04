using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;
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

        public async Task<APITask> GetTask(Guid id, CancellationToken cancellationToken = default)
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
                            }
                        }
                   ",
                    OperationName = "Task",
                    Variables = new
                    {
                        id = id,
                    }
                };

                var response = await _client.SendQueryAsync<TaskResponse>(request, cancellationToken);

                if (response.Errors == null)
                {
                    var task = new APITask(id, response.Data.Task.Title, response.Data.Task.Enabled);

                    return task;
                }
                else
                {
                    return null;
                }
                
            }
        }
    }
}
