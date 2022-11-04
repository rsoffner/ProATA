using Microsoft.Extensions.Configuration;
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
        public void ReturnTask()
        {
            var repo = new SqlTaskRepository(_configuration);
            Assert.IsNotNull(repo.GetTask(new Guid("4A652603-5AB5-403A-872D-ACEC009EE43F")));
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
