using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubDivisions
{
    public class GetCountrySubDivisionByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int CountrySubDivisionId { get; set; }
        }

        public class Response
        {
            public CountrySubDivisionDto CountrySubDivision { get; set; }
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
                        CountrySubDivision = await connection.QuerySingleProcAsync<CountrySubDivisionDto>("[Comsense].[ProcCountrySubDivisionGet]", new { request.CountrySubDivisionId })
                    };
                }
            }
        }
    }
}
