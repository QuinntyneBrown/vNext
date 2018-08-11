using vNext.Core.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using vNext.Core.Models;
using vNext.Core.Extensions;
using vNext.Core.Common;

namespace vNext.API.Features.DashboardTiles
{
    public class GetDashboardTileByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int DashboardTileId { get; set; }
        }

        public class Response
        {
            public DashboardTileDto DashboardTile { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {

                using(var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var dashboardTile = await connection.QuerySingleProcAsync<DashboardTile>($"[Common].[ProcDashboardTileGet]", new { request.DashboardTileId });
                    
                    return new Response()
                    {
                        DashboardTile = DashboardTileDto.FromDashboardTile(dashboardTile)
                    };
                }
            }
        }
    }
}
