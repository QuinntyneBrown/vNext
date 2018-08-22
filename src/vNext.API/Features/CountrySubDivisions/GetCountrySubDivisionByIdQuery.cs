using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubdivisions
{
    public class GetCountrySubdivisionByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int CountrySubdivisionId { get; set; }
        }

        public class Response
        {
            public CountrySubdivisionDto CountrySubdivision { get; set; }
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
                        CountrySubdivision = await connection.QuerySingleProcAsync<CountrySubdivisionDto>("[Comsense].[ProcCountrySubdivisionGet]", new { request.CountrySubdivisionId })
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
