using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Dashboards
{
    public class DashboardGetByUserIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public int UserId { get; set; }
        }

        public class Response
        {
            public IEnumerable<DashboardDto> Dashboards { get; set; }
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
                    var dashboardProjections = await Procedure.ExecuteAsync(request, connection);
                    IEnumerable<DashboardDto> dashboards = dashboardProjections.Select(x => DashboardDto.FromDashboard(x));

                    return new Response()
                    {
                        Dashboards = dashboards
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                var dashboards = await connection.QueryProcAsync<QueryProjectionDto>("[Common].[ProcDashboardGetByUserId]", new { request.UserId });

                foreach (var dashboard in dashboards) {
                    dashboard.DashboardTiles = await DashboardTiles.GetDashboardTilesByDashboardIdQuery.Procedure.ExecuteAsync(new DashboardTiles.GetDashboardTilesByDashboardIdQuery.Request()
                    {
                        DashboardId = dashboard.DashboardId
                    }, connection);
                }

                return dashboards;
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
