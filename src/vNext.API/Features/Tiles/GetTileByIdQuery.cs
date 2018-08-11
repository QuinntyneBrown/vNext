using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace vNext.API.Features.Tiles
{
    public class GetTileByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int TileId { get; set; }
        }

        public class Response
        {
            public TileDto Tile { get; set; }
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
                    return new Response()
                    {
                        Tile = await connection.QuerySingleAsync<TileDto>("[Comsense].[ProcTileGet]", new { request.TileId }, commandType: CommandType.StoredProcedure)
                    };
                }
            }
        }
    }
}
