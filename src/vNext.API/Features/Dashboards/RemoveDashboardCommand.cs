using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using vNext.Core.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace vNext.API.Features.Dashboards
{
    public class RemoveTaxCommand
    {
        public class Request : IRequest
        {
            public DashboardDto Dashboard { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }


            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
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
