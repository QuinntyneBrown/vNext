using vNext.Core.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using vNext.Core.Models;
using vNext.Core.Extensions;

namespace vNext.API.Features.DashboardTiles
{
    public class GetDashboardTileByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int DashboardTileId { get; set; }
        }

        public class Response
        {
            public DashboardTileDto DashboardTile { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {

                using(var connection = _sqlConnectionManager.GetConnection())
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
