using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Regions
{
    public class GetRegionByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int RegionId { get; set; }
        }

        public class Response
        {
            public RegionDto Region { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                    return new Response()
                    {
                        Region = RegionDto.FromRegion(await connection.QuerySingleProcAsync<dynamic>("[Common].[ProcRegionGet]", new { request.RegionId }))
                    };
            }
        }
    }
}
