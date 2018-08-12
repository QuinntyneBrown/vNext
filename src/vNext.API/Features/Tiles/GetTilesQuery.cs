using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using vNext.Core.Extensions;
using vNext.Core.Common;
using System.Data;

namespace vNext.API.Features.Tiles
{
    public class GetTilesQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<TileDto> Tiles { get; set; }
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
                    return new Response()
                    {
                        Tiles = await Procedure.ExecuteAsync(request, connection)
                    };
                }
            }

            public static class Procedure
            {
                public static async Task<IEnumerable<TileDto>> ExecuteAsync(Request request, IDbConnection connection)
                {
                    return await connection.QueryProcAsync<TileDto>("[Comsense].[ProcTileGetAll]", new { });
                }
            }
        }
    }
}
