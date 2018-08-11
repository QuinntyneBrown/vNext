using MediatR;
using System.Threading.Tasks;
using System.Threading;
using vNext.Core.Models;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace vNext.API.Features.Dashboards
{
    public class GetDashboardByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int DashboardId { get; set; }
        }

        public class Response
        {
            public DashboardDto Dashboard { get; set; }
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
                    var dashboard = await Procedure.ExecuteAsync(request, connection);

                    return new Response()
                    {
                        Dashboard = DashboardDto.FromDashboard(dashboard)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                var dashboard = await connection.QuerySingleProcAsync<QueryProjectionDto>($"[Common].[ProcDashboardGet]", new { request.DashboardId });

                dashboard.DashboardTiles = await DashboardTiles.GetDashboardTilesByDashboardIdQuery.Procedure.ExecuteAsync(new DashboardTiles.GetDashboardTilesByDashboardIdQuery.Request()
                {
                    DashboardId = dashboard.DashboardId
                }, connection);

                return dashboard;
            }
        }

        public class QueryProjectionDto
        {
            public int DashboardId { get; set; }
            public string Code { get; set; }
            public int UserId { get; set; }
            public string Settings { get; set; }
            public int ConcurrencyVersion { get; set; }

            public IEnumerable<DashboardTiles.GetDashboardTilesByDashboardIdQuery.QueryProjectionDto> DashboardTiles { get; set; }
            = new HashSet<DashboardTiles.GetDashboardTilesByDashboardIdQuery.QueryProjectionDto>();
        }
    }
}
