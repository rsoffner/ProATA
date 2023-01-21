using Microsoft.Extensions.Configuration;

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

        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }

    }
}
