using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.DashboardTiles
{
    public class DashboardTileEstimateFetchQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> {
            public int DashboardTileId { get; set; }
        }

        public class Response
        {
            public IEnumerable<DashboardTileEstimateDto> DashboardTileEstimates { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<QueryProjectionDto>> _procedure;
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
                        DashboardTileEstimates = (await Procedure.ExecuteAsync(request, connection))
                        .Select(x => DashboardTileEstimateDto.FromDashboardTileEstimate(x)) as IEnumerable<DashboardTileEstimateDto>
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Common].[ProcDashboardTileEstimateFetch]", new { request.DashboardTileId });
            }
        }

        public class QueryProjectionDto
        {
            public int DashboardTileEstimateId { get; set; }
            public string Code { get; set; }
        }
    }
}
