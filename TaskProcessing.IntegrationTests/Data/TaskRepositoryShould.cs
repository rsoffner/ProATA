using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using TaskProcessing.Data.Entities;
using TaskProcessing.Data.GraphQL;
using TaskProcessing.Data.Repositories;

namespace TaskProcessing.IntegrationTests.Data
{
    [TestFixture]
    public class TaskRepositoryShould
    {
        private IConfiguration _configuration;

        public TaskRepositoryShould()
        {
            _configuration = InitConfiguration();
        }

        [Test]
        public void RunGraphQL()
        {
            using (GraphQLHttpClient _client = new GraphQLHttpClient("https://localhost:7173/graphql", new NewtonsoftJsonSerializer()))
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
                        id = "4A652603-5AB5-403A-872D-ACEC009EE43F",
                    }
                };

                var response = _client.SendQueryAsync<TaskResponse>(request);

                var result = response.Result;

 
            }
        }

        [Test]
        public void ReturnTask()
        {
            var repo = new GraphQLTaskRepository(_configuration);

            var task = repo.GetTask(new Guid("4A652603-5AB5-403A-872D-ACEC009EE43F"));

            var response = task;
            
            Assert.IsNotNull(task);
        }

        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }

    }
}
