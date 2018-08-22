using MediatR;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Concurrencies
{
    public class GetConcurrenciesQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public Response(IEnumerable<ConcurrencyDto> concurrencies)
                => Concurrencies = concurrencies;
            public IEnumerable<ConcurrencyDto> Concurrencies { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<ConcurrencyDto>> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response(await connection.QueryProcAsync<ConcurrencyDto>("[Comsense].[ProcConcurrencyGet]"));
                }
            }
        }

        public class Procedure : IProcedure<Request, ConcurrencyDto>
        {
            public async Task<ConcurrencyDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
