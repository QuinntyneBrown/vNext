using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

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
            private readonly IProcedure<Request, IEnumerable<TileDto>> _procedure;
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

            public class Procedure
            {
                public static async Task<IEnumerable<TileDto>> ExecuteAsync(Request request, IDbConnection connection)
                {
                    return await connection.QueryProcAsync<TileDto>("[Comsense].[ProcTileGetAll]", new { });
                }
            }
        }
    }
}
