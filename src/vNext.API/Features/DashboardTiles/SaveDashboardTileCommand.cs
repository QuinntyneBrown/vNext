using Dapper;
using MediatR;
using Newtonsoft.Json;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Interfaces;

namespace vNext.API.Features.DashboardTiles
{
    public class SaveDashboardTileCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public DashboardTileDto DashboardTile { get; set; }
        }

        public class Response
        {			
            public int DashboardTileId { get; set; }
            public int ConcurrencyVersion { get; set; }
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
                {

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.AddDynamicParams(new
                    {
                        request.DashboardTile.DashboardTileId,
                        request.DashboardTile.DashboardId,
                        request.DashboardTile.TileId,
                        Settings = ParseSettings(request.DashboardTile)
                    });

                    var parameterDirection = request.DashboardTile.DashboardTileId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("DashboardTileId", request.DashboardTile.DashboardTileId, DbType.Int16, parameterDirection);

                    await connection.ExecuteAsync("[Common].[ProcDashboardTileSave]", dynamicParameters, commandType: CommandType.StoredProcedure);

                    var dashboardTileId = dynamicParameters.Get<short>("@DashboardTileId");

                    return new Response()
                    {
                        DashboardTileId = dashboardTileId
                    };
                }                
            }

            public string ParseSettings(DashboardTileDto model)
            {
                var json = JsonConvert.SerializeObject(model.Settings);

                switch (model.TileId)
                {
                    case 6:
                        return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<EstimateDashboardTileSettingsDto>(json));

                    default:
                        return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<DashboardTileSettingsDto>(json));
                }
            }

        }


    }
}
