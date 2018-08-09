using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Concurrencies
{
    public class GetConcurrenciesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public Response(IEnumerable<ConcurrencyDto> concurrencies)
                => Concurrencies = concurrencies;
            public IEnumerable<ConcurrencyDto> Concurrencies { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response(await connection.QueryProcAsync<ConcurrencyDto>("[Comsense].[ProcConcurrencyGet]"));
                }
            }
        }
    }
}
