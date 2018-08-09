using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using vNext.Core.Extensions;

namespace vNext.API.Features.Tiles
{
    public class GetTilesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<TileDto> Tiles { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        Tiles = await connection.QueryProcAsync<TileDto>("[Comsense].[ProcTileGetAll]")
                    };
                }
            }
        }
    }
}
