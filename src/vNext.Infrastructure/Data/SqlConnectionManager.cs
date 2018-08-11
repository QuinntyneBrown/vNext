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
            var table = tableClient.GetTableReference("Customers");
            var query = new TableQuery<TableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "1"));
            var resultSegment = table.ExecuteQuerySegmentedAsync(query, null).GetAwaiter().GetResult();
            var tableEntity = resultSegment.Results.SingleOrDefault(x => x.CustomerKey == key);
            var template = "Server={0};Initial Catalog={1};Integrated Security={2};Persist Security Info=False;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Application Name=Comsense Enterprise - {3};";
            var connectionString = string.Format(template, tableEntity.DatabaseServerName, tableEntity.DatabaseName, string.IsNullOrEmpty(tableEntity.DatabaseUserName) ? "True" : "False", key ?? string.Empty);
            if (!string.IsNullOrEmpty(tableEntity.DatabaseUserName) && !string.IsNullOrEmpty(tableEntity.DatabasePassword))
                connectionString = $"{connectionString}User ID = {tableEntity.DatabaseUserName};Password={tableEntity.DatabasePassword};";
            return connectionString;
        }
    }

    public class TableEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public string CustomerKey { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseServerName { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
    }
}
