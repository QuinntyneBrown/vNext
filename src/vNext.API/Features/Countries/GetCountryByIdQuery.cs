using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Countries
{
    public class GetCountryByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int CountryId { get; set; }
        }

        public class Response
        {
            public CountryDto Country { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        Country = await connection.QuerySingleProcAsync<CountryDto>("[Comsense].[ProcCountryGet]", new { request.CountryId })
                    };
                }
            }
        }
    }
}
