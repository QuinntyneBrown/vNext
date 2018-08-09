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
    public class ConcurrencyDeleteCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Concurrency.ConcurrencyId).NotNull();                
            }
        }

        public class Request : IRequest<Response> {
            public ConcurrencyDto Concurrency { get; set; }
        }

        public class Response
        {
            public Response(short concurrencyId) => ConcurrencyId = concurrencyId;

            public int ConcurrencyId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler( ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = default(short);

                    using (var connection = _sqlConnectionManager.GetConnection())
                    {
                        connection.Open();

                        result = await new Procedure().ExecuteAsync(request, connection);
                    }

                    transaction.Complete();

                    return new Response(result);
                }
            }
        }

        public class Procedure
        {
            public async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new { request.Concurrency.ConcurrencyId });

                var parameterDirection = request.Concurrency.ConcurrencyId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("ConcurrencyId", request.Concurrency.ConcurrencyId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Comsense].[ProcConcurrencyDelete]", dynamicParameters);

                return dynamicParameters.Get<short>("@ConcurrencyId");
            }
        }
    }
}
