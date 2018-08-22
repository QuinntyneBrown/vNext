using MediatR;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubdivisions
{
    public class GetCountrySubdivisionsQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CountrySubdivisionDto> CountrySubdivisions { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<CountrySubdivisionDto>> _procedure;
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
                        CountrySubdivisions = await connection.QueryProcAsync<CountrySubdivisionDto>("[Comsense].[ProcCountrySubdivisionGetAll]")
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, CountrySubdivisionDto>
        {
            public async Task<CountrySubdivisionDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
