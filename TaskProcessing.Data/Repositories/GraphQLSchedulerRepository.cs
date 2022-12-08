using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using System.Threading;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.GraphQL;

namespace TaskProcessing.Data.Repositories
{
    public class GraphQLSchedulerRepository : ISchedulerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;

        public GraphQLSchedulerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration["AppSettings:EndpointUrl"];
        }

        public void AddScheduler(Scheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public Scheduler GetById(Guid schedulerId)
        {
            using (GraphQLHttpClient _client = new GraphQLHttpClient(_url, new NewtonsoftJsonSerializer()))
            {
                var request = new GraphQLHttpRequest
                {
                    Query = @"
                        query Scheduler($id: UUID!) {
                            scheduler(id: $id) {
                                hostName
                                defaultHost
                            }
                        }
                   ",
                    OperationName = "Scheduler",
                    Variables = new
                    {
                        id = schedulerId,
                    }
                };

                var response = _client.SendQueryAsync<SchedulerResponse>(request);

                if (response.Result.Errors == null)
                {
                    var scheduler = new Scheduler(schedulerId, response.Result.Data.Scheduler.HostName, response.Result.Data.Scheduler.DefaultHost);

                    return scheduler;
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<Scheduler> GetSchedulers()
        {
            using (GraphQLHttpClient _client = new GraphQLHttpClient(_url, new NewtonsoftJsonSerializer()))
            {
                var request = new GraphQLHttpRequest
                {
                    Query = @"
                        {
                            schedulers {
                                id
                                hostName
                                defaultHost
                            }
                        }
                   "
                };

                var response = _client.SendQueryAsync<SchedulersResponse>(request);

                IList<Scheduler> schedulers = new List<Scheduler>();
                if (response.Result.Errors == null)
                {
                    
                    foreach (var scheduler in response.Result.Data.Schedulers)
                    {
                        schedulers.Add(new Scheduler(scheduler.Id, scheduler.HostName, scheduler.DefaultHost));
                    }
                }

                return schedulers;
            }
        }
    }
}
