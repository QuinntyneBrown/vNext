using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Tiles
{
    public class SaveTileCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public TileDto Tile { get; set; }
        }

        public class Response
        {			
            public int TileId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
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
                        request.Tile.TileId,
                        request.Tile.Code
                    });

                    var parameterDirection = request.Tile.TileId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("TileId",  request.Tile.TileId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteAsync("[Comsense].[ProcTileSave]", dynamicParameters, commandType: CommandType.StoredProcedure);
                    
                    return new Response()
                    {
                        TileId = dynamicParameters.Get<short>("@TileId")
                    };
                }
            }

        }
    }
}
