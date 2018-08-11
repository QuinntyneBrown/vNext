using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class CustomerSetting : TableEntity
    {
        public string CustomerKey { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseServerName { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
    }

    public class CustomerSettingsScenarios: ScenarioBase
    {
        [Fact]
        public async Task ShouldGet()
        {
            using (var server = CreateServer())
            {
                var customerKey = "QUINNTYNE_DEV";
                var configuration = server.Host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
                var storageAccount = CloudStorageAccount.Parse(configuration["Storage:DefaultConnection:StorageConnectionString"]);
                var tableClient = storageAccount.CreateCloudTableClient();
                var customers = tableClient.GetTableReference("Customers");
                var query = new TableQuery<CustomerSetting>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "1"));
                var resultSegment = await customers.ExecuteQuerySegmentedAsync(query, null);
                var item = resultSegment.Results.SingleOrDefault(x => x.CustomerKey == customerKey);
                var connectionString = $"Data Source={item.DatabaseServerName};Initial Catalog={item.DatabaseName};Integrated Security=SSPI;Connection Timeout=30;";                
                Assert.Equal("Data Source=localhost\\SQL2017;Initial Catalog=ComsenseDataDev;Integrated Security=SSPI;Connection Timeout=30;", connectionString);
            }
        }

        [Fact]
        public async Task ShouldAlsoGet()
        {
            using (var server = CreateServer())
            {
                var customerKey = "UAT";                
                var configuration = server.Host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
                var storageAccount = CloudStorageAccount.Parse(configuration["Storage:DefaultConnection:StorageConnectionString"]);
                var tableClient = storageAccount.CreateCloudTableClient();
                var customers = tableClient.GetTableReference("Customers");
                var query = new TableQuery<CustomerSetting>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "1"));
                var resultSegment = await customers.ExecuteQuerySegmentedAsync(query, null);
                var item = resultSegment.Results.SingleOrDefault(x => x.CustomerKey == customerKey);

                var template = "Server={0};Initial Catalog={1};Integrated Security={2};Persist Security Info=False;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Application Name=Comsense Enterprise - {3};";

                var connectionString = string.Format(template,item.DatabaseServerName,item.DatabaseName, string.IsNullOrEmpty(item.DatabaseUserName) ? "True" : "False", customerKey ?? string.Empty);

                if (!string.IsNullOrEmpty(item.DatabaseUserName) && !string.IsNullOrEmpty(item.DatabasePassword))
                    connectionString = $"{connectionString}User ID = {item.DatabaseUserName};Password={item.DatabasePassword};";

                Assert.NotNull(connectionString);
            }
        }
    }
}
