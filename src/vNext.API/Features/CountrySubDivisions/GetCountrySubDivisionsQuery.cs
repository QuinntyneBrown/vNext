using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubDivisions
{
    public class GetCountrySubDivisionsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CountrySubDivisionDto> CountrySubDivisions { get; set; }
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
                        CountrySubDivisions = await connection.QueryProcAsync<CountrySubDivisionDto>("[Comsense].[ProcCountrySubDivisionGetAll]")
                    };
                }
            }
        }
    }
}
