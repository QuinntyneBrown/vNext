using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.ShipTos
{
    public class GetShipToByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int ShipToId { get; set; }
        }

        public class Response
        {
            public ShipToDto ShipTo { get; set; }
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
                        ShipTo = ShipToDto.FromShipTo((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Common].[ProcShipToGet]", new { request.ShipToId });
            }
        }

        public class QueryProjectionDto
        {
            public int ShipToId { get; set; }
            public string Code { get; set; }
        }
    }
}
