using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using vNext.API.Features.Concurrencies;

namespace vNext.API.Features
{
    public static class SaveEntityCommandHandler
    {
        public static async Task<TResponse> Handle<TRquest, TResponse>(TRquest request, Func<TRquest, IDbConnection,Task<TResponse>> procedure, string domain, int id, int version, IDbConnection connection)
        {
            var newVersion = (await ConcurrencyGetVersionByDomainAndIdQuery.Procedure
                .ExecuteAsync(new ConcurrencyGetVersionByDomainAndIdQuery.Request()
                {
                    Id = id,
                    Version = version,
                    Domain = domain
                }, connection));

            if (version < newVersion)
                throw new Exception("Concurrency Error");

            var result = await procedure(request, connection);

            await ConcurrencySaveCommand.Procedure
                .ExecuteAsync(new ConcurrencySaveCommand.Request()
                {
                    Concurrency = new ConcurrencyDto()
                    {
                        Domain = domain,
                        Id = Convert.ToInt32(result),
                        Version = version,
                    }
                }, connection);

            return result;
        }
    }
}
