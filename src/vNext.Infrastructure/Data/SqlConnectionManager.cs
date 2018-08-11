using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using vNext.Core.Interfaces;

namespace vNext.Infrastructure.Data
{
    public class SqlConnectionManager : IDbConnectionManager
    {
        private readonly IConfiguration _configuration;
        private static readonly object sync = new object();
        private Dictionary<string, string> _connectionStringDictionary = new Dictionary<string, string>();

        public SqlConnectionManager(IConfiguration configuration)
            => _configuration = configuration;

        public IDbConnection GetConnection(string key)
        {
            lock (sync)
            {
                _connectionStringDictionary.TryGetValue(key, out var connectionString);
                if(string.IsNullOrEmpty(connectionString))
                {
                    connectionString = GetConnectionString(key);
                    _connectionStringDictionary.TryAdd(key, connectionString);
                }
                return new SqlConnection(connectionString);
            }
        }

        public string GetConnectionString(string key)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration["Storage:DefaultConnection:StorageConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();
            var customers = tableClient.GetTableReference("Customers");
            var query = new TableQuery<CustomerSetting>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "1"));
            var resultSegment = customers.ExecuteQuerySegmentedAsync(query, null).GetAwaiter().GetResult();
            var customer = resultSegment.Results.SingleOrDefault(x => x.CustomerKey == key);
            var template = "Server={0};Initial Catalog={1};Integrated Security={2};Persist Security Info=False;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Application Name=Comsense Enterprise - {3};";
            var connectionString = string.Format(template, customer.DatabaseServerName, customer.DatabaseName, string.IsNullOrEmpty(customer.DatabaseUserName) ? "True" : "False", key ?? string.Empty);
            if (!string.IsNullOrEmpty(customer.DatabaseUserName) && !string.IsNullOrEmpty(customer.DatabasePassword))
                connectionString = $"{connectionString}User ID = {customer.DatabaseUserName};Password={customer.DatabasePassword};";
            return connectionString;
        }
    }

    public class CustomerSetting : TableEntity
    {
        public string CustomerKey { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseServerName { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
    }
}
