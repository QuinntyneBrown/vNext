using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace vNext.Core.Extensions
{
    public static class DbConnectionExtensions
    {
        public static async Task<int> ExecuteProcAsync(this IDbConnection connection, string sql, object dynamicParameters = null, IDbTransaction dbTransaction = null)
        {
            return await connection.ExecuteAsync(sql, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: dbTransaction);
        }

        public static async Task<IEnumerable<T>> QueryProcAsync<T>(this IDbConnection sqlConnection, string sql, object dynamicParameters = null)
        {
            return await sqlConnection.QueryAsync<T>(sql, dynamicParameters, commandType: CommandType.StoredProcedure);
        }

        public static async Task<T> QuerySingleProcAsync<T>(this IDbConnection sqlConnection, string sql, object dynamicParameters = null)
        {
            return await sqlConnection.QuerySingleAsync<T>(sql, dynamicParameters, commandType: CommandType.StoredProcedure);
        }
    }
}
