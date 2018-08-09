using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Concurrencies
{
    public class ConcurrencyGetVersionByDomainAndIdQuery
    {
        public class Request : IRequest<Response> {
            public int Version { get; set; }
            public string Domain { get; set; }
            public int Id { get; set; }
        }

        public class Response
        {
            public Response(short version) => Version = version;
            public short Version { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
			    => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = default(short);

                    using (var connection = _sqlConnectionManager.GetConnection())
                    {
                        connection.Open();

                        result = await Procedure.ExecuteAsync(request, connection);
                    }

                    transaction.Complete();

                    return new Response(result);
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.Domain,
                    request.Id,
                    request.Version,
                });

                dynamicParameters.Add("Version", request.Version, DbType.Int16, ParameterDirection.InputOutput);

                await connection.ExecuteProcAsync("[Comsense].[ProcConcurrencyGetVersionByDomainAndId]", dynamicParameters);

                return dynamicParameters.Get<short>("@Version");
            }
        }
    }
}
