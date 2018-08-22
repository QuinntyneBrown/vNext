using Dapper;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using System;

namespace vNext.API.Features.DashboardTiles
{
    public class RemoveDashboardTileCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int DashboardTileId { get; set; }
            public int ConcurrencyVersion { get; set; }
        }

        public class Response
        {
            public int DashboardTileId { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var favioriteResult = await DashboardTileFavoriteDeleteByDashboardTileIdCommand.Procedure.ExecuteAsync(new DashboardTileFavoriteDeleteByDashboardTileIdCommand.Request()
                    {
                        DashboardTileId = request.DashboardTileId
                    }, connection);

                    var pinnedResult = await DashboardTilePinnedDeleteByDashboardTileIdCommand.Procedure.ExecuteAsync(new DashboardTilePinnedDeleteByDashboardTileIdCommand.Request()
                    {
                        DashboardTileId = request.DashboardTileId
                    }, connection);

                    var result = await connection.ExecuteProcAsync("[Common].[ProcDashboardTileDelete]", new { request.DashboardTileId });

                    return new Response()
                    {
                        DashboardTileId = result
                    };
                }               
            }
        }

        public class Procedure : IProcedure<Request, short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
