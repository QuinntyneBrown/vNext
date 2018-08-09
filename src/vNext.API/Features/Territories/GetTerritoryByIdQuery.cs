using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Territories
{
    public class GetTerritoryByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int TerritoryId { get; set; }
        }

        public class Response
        {
            public TerritoryDto Territory { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
			    => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        Territory = TerritoryDto.FromTerritory((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Common].[ProcTerritoryGet]", new { request.TerritoryId });
            }
        }

        public class QueryProjectionDto
        {
            public int TerritoryId { get; set; }
            public string Code { get; set; }
        }
    }
}
