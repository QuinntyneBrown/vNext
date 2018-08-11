using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Concurrencies
{
    public class ConcurrencySaveCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Concurrency.ConcurrencyId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public ConcurrencyDto Concurrency { get; set; }
        }

        public class Response
        {
            public Response(short concurrencyId) => ConcurrencyId = concurrencyId;

            public int ConcurrencyId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = default(short);

                    using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
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
            public static async Task<short> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.Concurrency.Domain,
                    request.Concurrency.Id,
                    request.Concurrency.Version,
                });

                dynamicParameters.Add("ConcurrencyId", null, DbType.Int16, ParameterDirection.Output);

                dynamicParameters.Add("Version", null, DbType.Int16, ParameterDirection.InputOutput);

                await connection.ExecuteProcAsync("[Comsense].[ProcConcurrencySave]", dynamicParameters);

                return dynamicParameters.Get<short>("@Version");
            }
        }
    }
}
