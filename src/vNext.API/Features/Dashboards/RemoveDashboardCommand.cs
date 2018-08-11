using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using vNext.Core.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using vNext.Core.Common;

namespace vNext.API.Features.Dashboards
{
    public class RemoveTaxCommand
    {
        public class Request : AuthenticatedRequest, IRequest
        {
            public DashboardDto Dashboard { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }


            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var dashboard = await connection.QuerySingleProcAsync<Dashboard>($"[Common].[ProcDashboardGet]", new { request.Dashboard.DashboardId });

                    foreach (var dashboardTile in await connection.QueryProcAsync<DashboardTile>("[Common].[ProcDashboardTileGetByDashboardId]", new { request.Dashboard.DashboardId }))
                    {
                        await connection.ExecuteProcAsync("[Common].[ProcDashboardTileDelete]", new { dashboardTile.DashboardTileId });
                    }

                    await connection.ExecuteProcAsync("[Common].[ProcDashboardDelete]", new { request.Dashboard.DashboardId });
                }                
            }
        }
    }
}
