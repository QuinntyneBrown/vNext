using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.DashboardTiles
{
    public class GetDashboardTilesByDashboardIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int DashboardId { get; set; }
        }

        public class Response
        {
            public IEnumerable<DashboardTileDto> DashboardTiles { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;

            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var dashboardTiles = await Procedure.ExecuteAsync(request, connection);

                    return new Response()
                    {
                        DashboardTiles = dashboardTiles.Select(d => DashboardTileDto.FromDashboardTile(d))
                    };
                }
            }            
        }

        public class QueryProjectionDto
        {
            public int DashboardId { get; set; }
            public int DashboardTileId { get; set; }
            public int TileId { get; set; }
            public string Settings { get; set; }
            public int ConcurrencyVersion { get; set; }
        }

        public static class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
                => await connection.QueryProcAsync<QueryProjectionDto>("[Common].[ProcDashboardTileGetByDashboardId]", new { request.DashboardId });
        }
    }
}
