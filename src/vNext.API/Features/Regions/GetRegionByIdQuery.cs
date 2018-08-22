using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Regions
{
    public class GetRegionByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int RegionId { get; set; }
        }

        public class Response
        {
            public RegionDto Region { get; set; }
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
                    return new Response()
                    {
                        Region = RegionDto.FromRegion(await connection.QuerySingleProcAsync<RegionDto>("[Common].[ProcRegionGet]", new { request.RegionId }))
                    };
            }
        }

        public class Procedure : IProcedure<Request, RegionDto>
        {
            public async Task<RegionDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
