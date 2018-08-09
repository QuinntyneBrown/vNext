using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Countries
{
    public class GetCountryByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int CountryId { get; set; }
        }

        public class Response
        {
            public CountryDto Country { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
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
