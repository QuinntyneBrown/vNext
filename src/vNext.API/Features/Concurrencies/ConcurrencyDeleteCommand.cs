using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
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

        public class Request : AuthenticatedRequest, IRequest<Response> {
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
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    connection.Open();

                    return new Response(await Procedure.ExecuteAsync(request, connection));
                }                
            }
        }

        public class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
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
